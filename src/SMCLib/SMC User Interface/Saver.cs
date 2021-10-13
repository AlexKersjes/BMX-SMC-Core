using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Numerics;
using System.Text.Json.Serialization;
using System.IO;

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

        class RunData
        {
            public int RiderID;
            public DateTime EndOfRunTimestamp;
            public List<SensorData> sensors;
        }

        struct SensorData
        {
            public int id;
            public Vector3 CalibrationOffset;

            public List<DataPoint> points;
        }

        struct DataPoint
        {
            public float confidenceScore;
            public List<Vector3> accelerometer;
            public Vector3 coordinates;
            public float hardwareTimestamp;
            public long systemTimestamp;
            public float speed;
        }

        public string Jsonify(DateTime ts)
        {
            var run = new RunData
            {
                RiderID = this.RiderID,
                EndOfRunTimestamp = ts,
                sensors = new List<SensorData> { },
            };
            this.Sensors.ForEach(delegate (SMC_Core.PozyxSensor s) 
                {
                    run.sensors.Add(ConvertSensor(s));
                } ) ;
            string JSONstring = JsonSerializer.Serialize(run);
            return JSONstring;
        }

        private SensorData ConvertSensor (SMC_Core.PozyxSensor input)
        {
            SensorData list = new SensorData() { points = new List<DataPoint>(), id = input.GetID() };
            var ls = input.getAllPozyxData().ToList();
            ls.ForEach(delegate (SMC_Core.PozyxData p) 
            {
                list.points.Add(new DataPoint
                {
                    hardwareTimestamp = p.hardwareTimestamp,
                    systemTimestamp = p.systemTimestamp,
                    accelerometer = p.accelerometer,
                    coordinates = p.coordinates,
                    confidenceScore = p.score,
                    speed = p.speed
                });
            });
            return list;
        }


        public void Save()
        {
            DateTime now = DateTime.Now;
            string filename = RiderID + "$" + now.ToString("yyyyMMddTHHmmss")+".json";
            File.WriteAllText(filename, Jsonify(now));
            this.Sensors.ForEach(delegate (SMC_Core.PozyxSensor s) { s.clearData(); });
        }

    }
}
