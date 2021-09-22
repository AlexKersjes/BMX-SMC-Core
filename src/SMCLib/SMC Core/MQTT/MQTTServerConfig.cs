namespace SMC_Core
{
    /// <summary>
    /// Holds the configuration values needed when connecting to a MQTT broker
    /// </summary>
    public struct MQTTServerConfig
    {
        public string serverAddress;
        public string topic;
        public string username;
        public string password;
    }
}
