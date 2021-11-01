using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Text.Json.Serialization;
using System.IO;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;

namespace SMCLib
{
    class Saver
    {
        public int RiderID { get; private set; }
        private List<SMC_Core.PozyxSensor> Sensors = new List<SMC_Core.PozyxSensor> { };
        public Saver (int RiderID, List<SMC_Core.PozyxSensor> Sensors)
        {
            this.Sensors = Sensors;
            this.RiderID = RiderID;
        }

        public class RunData
        {
            public int RiderID { get; set; }
            public DateTime EndOfRunUTC { get; set; }
            public List<SensorData> Sensors { get; set; }
        }

        public struct SensorData
        {
            public int Id { get; set; }
            public Vector3 ClbrOffset { get; set; }
            public List<DataPoint> Points { get; set; }
        }

        public struct DataPoint
        {
            public float Confidence { get; set; }
            public List<Vector3> Accelerometer { get; set; }
            public Vector3 Coordinates { get; set; }
            public float HardwareTime { get; set; }
            public long SystemTime { get; set; }
            public float Speed { get; set; }
        }

        public string Bsonify(DateTime ts)
        {
            var run = new RunData
            {
                RiderID = this.RiderID,
                EndOfRunUTC = ts.ToUniversalTime(),
                Sensors = new List<SensorData> { },
            };
            this.Sensors.ForEach(delegate (SMC_Core.PozyxSensor s) 
                {
                    run.Sensors.Add(ConvertSensor(s));
                } ) ;
            using (MemoryStream ms = new MemoryStream())
            using (BsonDataWriter datawriter = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(datawriter, run);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private SensorData ConvertSensor (SMC_Core.PozyxSensor input)
        {
            SensorData list = new SensorData() { Points = new List<DataPoint>(), Id = input.GetID() };
            var ls = input.getAllPozyxData().ToList();
            ls.ForEach(delegate (SMC_Core.PozyxData p) 
            {
                list.Points.Add(new DataPoint
                {
                    HardwareTime = p.hardwareTimestamp,
                    SystemTime = p.systemTimestamp,
                    Accelerometer = p.accelerometer,
                    Coordinates = p.coordinates,
                    Confidence = p.score,
                    Speed = p.speed
                });
            });
            list.Points.OrderBy(p => p.HardwareTime);
            return list;
        }


        public void Save()
        {
            DateTime now = DateTime.Now;
            string filename = RiderID + "$" + now.ToString("yyyyMMddTHHmmss")+".bson";
            File.WriteAllText(filename, Bsonify(now));
            this.Sensors.ForEach(delegate (SMC_Core.PozyxSensor s) { s.clearData(); });
        }

    }
}
