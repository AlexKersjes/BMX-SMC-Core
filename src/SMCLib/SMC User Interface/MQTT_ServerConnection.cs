using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MQTTnet;
using MQTT_serverconfig;
namespace SMCLib
{
   public class MQTT_ServerConnection:IStreamProvider
    {
        public MQTT_ServerConnection()
        {

        }


        public void Connect()
        {

        }

        public void Disconnect()
        {
            
        }

        public IEnumerable<IStreamContainer> getAvailableStreamers()
        {
            throw new NotImplementedException();
        }
    }
}
