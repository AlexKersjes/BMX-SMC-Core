using System;
using System.Collections.Generic;
using System.Numerics;

namespace SMCLib
{
    public class PozyxSensor : Sensor
    {
        private Stream<PozyxData> streamInstance;
        private List<PozyxData> pozyxData = new List<PozyxData>();
        private Vector3 calibrationOffset;

        public PozyxSensor(int id, Stream<PozyxData> stream) : base(id)
        {
            streamInstance = stream;
        }

        /* Possible override (unused at the moment):
        public override int GetID()
        {
             
        }
        */

        public override void updateData()
        {
            foreach (PozyxData data in streamInstance.Read(streamInstance.DataAvailable()))
            {
                // place if inside the foreach to consume all data from the streamInstance
                if (active && data != null)
                {
                    data.coordinates -= calibrationOffset;
                    pozyxData.Add(data);
                }
            }
        }

        // TODO: Make this a generic function in Sensor Handler?
        public IEnumerable<PozyxData> getAllPozyxData()
        {
            return pozyxData;
        }

        public void setCalibrationOffset(Vector3 offset)
        {
            Vector3 temp = calibrationOffset - offset; // ensures the previous offset is removed first
            calibrationOffset = offset; // set the new offset
            foreach (PozyxData data in pozyxData)
            {
                data.coordinates += temp;
            }
        }
        protected static PozyxData interpolate(PozyxData lhs, PozyxData rhs, float interpolationRate)
        {
            return new PozyxData(
                System.Convert.ToInt32((lhs.systemTimestamp * interpolationRate) + (rhs.systemTimestamp * (1 - interpolationRate))),
                (lhs.score * interpolationRate) + (rhs.score * (1 - interpolationRate)),
                (lhs.acceleration * interpolationRate) + (rhs.acceleration * (1 - interpolationRate)),
                (lhs.coordinates * interpolationRate) + (rhs.coordinates * (1 - interpolationRate)),
                (lhs.hardwareTimestamp * interpolationRate) + (rhs.hardwareTimestamp * (1 - interpolationRate)),
                (lhs.speed * interpolationRate) + (rhs.speed * (1 - interpolationRate))
                );
        }

        private PozyxData getDatapointAtTime(float time)
        {
            for (int i = 0; i < pozyxData.Count - 2; i++)
            {
                // TODO: using systemTimestamp for now, but we migt want to change
                // to hardwareTimestamp later for better accuracy (though that
                // leaves the problem that the calculation for the interpolation
                // rate becomes impossible)
                float minValue = pozyxData[i].systemTimestamp;
                float maxValue = pozyxData[i + 1].systemTimestamp;
                if (minValue >= time && maxValue <= time)
                {
                    float a = pozyxData[i].systemTimestamp;
                    float b = pozyxData[i + 1].systemTimestamp;
                    float interpolationRate = 100 - ((time - a) / (b - a) * 100);
                    return interpolate(pozyxData[i], pozyxData[i + 1], interpolationRate);
                }
            }
            return pozyxData[pozyxData.Count - 1];
        }
        /* 
         * TODO: the data should be updated every once in a while. 
         * Can this be done by an external function call?
         * or do we want to call this function when trying to
         * execute e.g. getSpeed, getPosition, etc.?
        */
        public override void clearData()
        {
            pozyxData.Clear();
            System.GC.Collect();
        }

        public float GetSpeed()
        {
            return pozyxData[pozyxData.Count - 1].speed;
        }
        public Vector3 GetPosition()
        {
            return pozyxData[pozyxData.Count - 1].coordinates;
        }
        public Vector3 GetAcceleration()
        {
            return pozyxData[pozyxData.Count - 1].acceleration;
        }

        public float GetSpeed(float timestamp)
        {
            return getDatapointAtTime(timestamp).speed;
        }
        public Vector3 GetPosition(float timestamp)
        {
            return getDatapointAtTime(timestamp).coordinates;
        }
        public Vector3 GetAcceleration(float timestamp)
        {
            return getDatapointAtTime(timestamp).acceleration;
        }

        public override string ToString()
        {
            return Convert.ToString(streamInstance.GetStreamerID());
        }
    }
}
