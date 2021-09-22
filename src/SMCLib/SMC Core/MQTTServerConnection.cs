using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace SMC_Core
{
    public class MQTTServerConnection : IStreamProvider
    {
        public IMqttClient client { get; set; }
        private MQTTServerConfig config;

        private StreamHandler handler;

        private JsonSerializerSettings settings;

        public MQTTServerConnection(MQTTServerConfig config, StreamHandler handler)
        {
            this.config = config;
            this.handler = handler;

            settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
            settings.Converters.Add(new JsonPathConverter());
        }

        public void Connect()
        {

        }

        private async Task OnDataReceived(MqttApplicationMessageReceivedEventArgs args)
        {
            string payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

            try
            {
                // get data property from json 
                JArray arr = JArray.Parse(payload);


                //  foreach (JObject dataObj in obj.DescendantsAndSelf().OfType<JObject>())
                // {

                //JObject dataObj = (JObject)obj.First;
                // do we add manual Value getters here or do we add json properties to the constructor
                JObject dataObj;
                dataObj = (JObject)arr.First;
                JObject child = dataObj.Value<JObject>("data");
                    if (child != null)
                    {
                        PozyxData pozyxData = JsonConvert.DeserializeObject<PozyxData>(dataObj.ToString(), settings);


                        JObject tagData = child.Value<JObject>("tagData");
                        // TODO: this is not very efficient. Add JsonProperty headers to Pozyxdata class
                        pozyxData.id = dataObj.Value<int>("tagId");
                        pozyxData.hardwareTimestamp = dataObj.Value<int>("timestamp");
                        pozyxData.score = dataObj.Value<int>("score");
                        if (pozyxData != null)
                        {
                            handler.AddData<PozyxData>(pozyxData);
                        }
                       // pozyxData.systemTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }

              //  }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async void connectRemote()
        {
            MqttFactory factory = new MqttFactory();
            client = new MqttFactory().CreateMqttClient();
            // FIXME: there were different optionbuilders depending on the connection type (local/remote)
            IMqttClientOptions options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer(config.serverAddress, 1883)
                .WithCredentials(config.username, config.password)
                .WithCleanSession()
                .Build();

            //this event gets called when connection is established
            client.UseConnectedHandler(async e =>
            {
                // Subscribe to a topic
                await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(config.topic).Build());
                //disconnectText.changeState();
            });
            // connects to the broker
            await client.ConnectAsync(options);
            client.UseApplicationMessageReceivedHandler(OnDataReceived);
        }

        public void Disconnect()
        {
            client.DisconnectAsync();
        }

        public IEnumerable<IStreamContainer> getAvailableStreamers()
        {
            throw new NotImplementedException();
        }
    }
}
