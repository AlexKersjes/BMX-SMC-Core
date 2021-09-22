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
        public PozyxSensor(Stream<PozyxData> stream)
        {
            streamInstance = stream;
        }

        /* Possible override (unused at the moment):
        public override int GetID()
        {
             
        }
        */
        // FIXME: these functions can probably be generic for all types of Sensor.
        protected override void updateData()
        {
            foreach (PozyxData data in streamInstance.Read(streamInstance.DataAvailable()))
            {
                data.position -= calibrationOffset;
                pozyxData.Add(data);
            }
        }

        public void setCalibrationOffset(Vector3 offset)
        {
            Vector3 temp = calibrationOffset - offset; // ensures the previous offset is removed first
            calibrationOffset = offset; // set the new offset
            foreach (PozyxData data in pozyxData)
            {
                data.position += temp;
            }
        }
        private PozyxData interpolate(PozyxData lhs, PozyxData rhs, float interpolationRate)
        {
            return new PozyxData(
                System.Convert.ToInt32((lhs.timestamp * interpolationRate) + (rhs.timestamp * (1 - interpolationRate))),
                (lhs.score * interpolationRate) + (rhs.score * (1 - interpolationRate)),
                // TODO: interpolate acceleration
                // (lhs.acceleration * interpolationRate) + (rhs.acceleration * (1 - interpolationRate)),
                new List<Vector3>(),
                (lhs.position * interpolationRate) + (rhs.position * (1 - interpolationRate)),
                (lhs.hardwareTimestamp * interpolationRate) + (rhs.hardwareTimestamp * (1 - interpolationRate)),
                (lhs.speed * interpolationRate) + (rhs.speed * (1 - interpolationRate))
                );
        }

        // TODO: could also be a generic function 
        // (classes should be required to implement the interpolate function themselves though)
        private PozyxData getDatapointAtTime(float time)
        {
            for (int i = 0; i < pozyxData.Count - 2; i++)
            {
                float minValue = pozyxData[i].timestamp;
                float maxValue = pozyxData[i + 1].timestamp;
                if (minValue >= time && maxValue <= time)
                {
                    float a = pozyxData[i].timestamp;
                    float b = pozyxData[i + 1].timestamp;
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
            return pozyxData[pozyxData.Count - 1].position;
        }
        public List<Vector3> GetAcceleration()
        {
            return pozyxData[pozyxData.Count - 1].acceleration;
        }

        public float GetSpeed(float timestamp)
        {
            return getDatapointAtTime(timestamp).speed;
        }
        public Vector3 GetPosition(float timestamp)
        {
            return getDatapointAtTime(timestamp).position;
        }
        public List<Vector3> GetAcceleration(float timestamp)
        {
            return getDatapointAtTime(timestamp).acceleration;
        }

        public override string ToString()
        {
            return Convert.ToString(streamInstance.GetStreamerID());
        }
    }
}
