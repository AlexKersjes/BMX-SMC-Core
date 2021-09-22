using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Constructor for the MQTTServerConnection class
        /// </summary>
        /// <param name="config">The configuration values for the server</param>
        /// <param name="handler">Data handler for sending recieved data to</param>
        public MQTTServerConnection(MQTTServerConfig config, StreamHandler handler)
        {
            this.config = config;
            this.handler = handler;

            settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
            settings.Converters.Add(new JsonPathConverter());
        }
        /// <summary>
        /// Event handler for recieving data
        /// </summary>
        /// <param name="args">The message data</param>
        private async Task OnDataReceived(MqttApplicationMessageReceivedEventArgs args)
        {
            string payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

            try
            {

                foreach (PozyxData data in Parsers.ParseJson(payload)) 
                {
                    if (data != null) 
                    {
                        handler.AddData<PozyxData>(data);
                    }
                }
                    // pozyxData.systemTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                

                //  }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// Connects to the remote server
        /// </summary>
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
            });
            // connects to the broker
            await client.ConnectAsync(options);
            client.UseApplicationMessageReceivedHandler(OnDataReceived);
        }
        /// <summary>
        /// Disconnects from the MQTT broker
        /// </summary>
        public void Disconnect()
        {
            client.DisconnectAsync();
        }

        public IEnumerable<IStreamContainer> getAvailableStreamers()
        {
            return handler.getAvailableStreamers(); // FIXME: why is this here?
        }
    }
}
