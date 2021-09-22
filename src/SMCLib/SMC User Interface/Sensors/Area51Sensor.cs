using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMCLib
{
    class Area51Sensor : Sensor
    {
        private Stream<Area51Data> streamInstance;
        private List<Area51Data> area51Data = new List<Area51Data>();

        public Area51Sensor(int id, Stream<Area51Data> stream) : base(id)
        {
            streamInstance = stream;
        }
        public override void clearData()
        {
            area51Data.Clear();
            System.GC.Collect();
        }

        public override void updateData()
        {
            foreach (Area51Data data in streamInstance.Read(streamInstance.DataAvailable()))
            {
                // place if inside the foreach to consume all data from the streamInstance
                if (active && data != null)
                {
                    area51Data.Add(data);
                }
            }
        }
    }
}
