using UnityEngine;
using System;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json.Linq;

public class Mqtt : Singleton<Mqtt>
{
    public IMqttClient client { get; set; }
    public string dataTagValue { get; set; }
    private JArray jsonVal;

    public async void connectLocal()
    {
        var factory = new MqttFactory();
        client = new MqttFactory().CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer("192.168.2.3")
            //.WithTcpServer("10.0.0.254") // Local connection
            .WithCleanSession()
            .Build();

        //this event gets called when connection is established
        client.UseConnectedHandler(async e =>
        {
            // Remove contents from csv as they become invalid for now
            LoadCSV.RecordedTagList.Clear();
            FindTags.foundTags.Clear();

            Debug.Log("### CONNECTED WITH SERVER ###");
            // Subscribe to a topic
            await client.SubscribeAsync(new TopicFilterBuilder().WithTopic("tags").Build());
            Debug.Log("### SUBSCRIBED ###");
            //disconnectText.changeState();
        });
        // connects to the broker
        await client.ConnectAsync(options);
    }
    public async void connectRemote()
    {
        var factory = new MqttFactory();
        client = new MqttFactory().CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithWebSocketServer("mqtt.cloud.pozyxlabs.com:443/mqtt")
            .WithCredentials("5bacbe8d72465b8d83f3e42b", "5a7d5889-264b-488d-9c63-9c052a470fda")
            .WithCleanSession()
            .WithTls()
            .Build();

        //this event gets called when connection is established
        client.UseConnectedHandler(async e =>
        {
            // Remove contents from csv as they become invalid for now
            LoadCSV.RecordedTagList.Clear();
            FindTags.foundTags.Clear();

            Debug.Log("### CONNECTED WITH SERVER ###");
            // Subscribe to a topic
            await client.SubscribeAsync(new TopicFilterBuilder().WithTopic("5bacbe8d72465b8d83f3e42b").Build());
            Debug.Log("### SUBSCRIBED ###");
            //disconnectText.changeState();
        });
        // connects to the broker
        await client.ConnectAsync(options);
    }

    public void Disconnect()
    {
        //disconnectText.changeState();
        if (client.IsConnected == true)
        {
            Debug.Log("Disconnected");
            client.DisconnectAsync();
        }

    }
    public bool isConnected()
    {
        bool status = client.IsConnected;
        return status;
    }

    public JArray getData()
    {
        if (client != null)
        {
            dataTagValue = "";
            client.UseApplicationMessageReceivedHandler(e =>
            {
                dataTagValue = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            });
            //only return if it has value
            if(dataTagValue != "")
            {
                jsonVal = JArray.Parse(dataTagValue);
                return jsonVal;
            }
        }
        return null;
    }

    public string getData(string FilePath)
    {

        return FilePath;
    }
}
