using System;
using System.Collections.Generic;
using System.Numerics;

namespace SMC_Core
{
    public class PozyxSensor : Sensor
    {
        private Stream<PozyxData> streamInstance;
        private List<PozyxData> pozyxData = new List<PozyxData>();
        private Vector3 calibrationOffset;

        /// <summary>
        /// Constructor for a pozyx position sensor
        /// </summary>
        /// <param name="id">The hardware ID of the sensor</param>
        /// <param name="stream">The stream from which to get data</param>
        public PozyxSensor(int id, Stream<PozyxData> stream) : base(id)
        {
            streamInstance = stream;
        }

        /* Possible override (unused at the moment):
        public override int GetID()
        {
             
        }
        */
        /// <summary>
        /// Retrieves data from the streamHandler and handles data calibration
        /// </summary>
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
        /// <summary>
        /// Gets all data for this sensor
        /// </summary>
        /// <returns>All data for this sensor</returns>
        public IEnumerable<PozyxData> getAllPozyxData()
        {
            return pozyxData;
        }
        /// <summary>
        /// Sets the position calibration offset for all data
        /// </summary>
        /// <param name="offset">The amount of offset to apply</param>
        public void setCalibrationOffset(Vector3 offset)
        {
            Vector3 temp = calibrationOffset - offset; // ensures the previous offset is removed first
            calibrationOffset = offset; // set the new offset
            foreach (PozyxData data in pozyxData)
            {
                data.coordinates += temp;
            }
        }
        /// <summary>
        /// Interpolates 2 data points
        /// </summary>
        /// <param name="lhs">Left hand side parameter</param>
        /// <param name="rhs">Right hand side parameter</param>
        /// <param name="interpolationRate">The percentage of the left/right hand side to use
        /// (e.g. when a you need a data point that is 25% lhs and 75% rhs, the interpolationRate is 25%)</param>
        /// <returns>A new, interpolated data point</returns>
        protected static PozyxData interpolate(PozyxData lhs, PozyxData rhs, float interpolationRate)
        {
            if (lhs.id != rhs.id)
            {
                // FIXME: probably should be less generic than "Exception"
                throw new Exception("make sure the 2 data points you're trying to interpolate have the same id.");
            }
            List<Vector3> accel = new List<Vector3>(lhs.accelerometer);
            accel.AddRange(rhs.accelerometer);
            return new PozyxData(
                System.Convert.ToInt32((lhs.systemTimestamp * interpolationRate) + (rhs.systemTimestamp * (1 - interpolationRate))),
                lhs.id,
                (lhs.score * interpolationRate) + (rhs.score * (1 - interpolationRate)),
                (accel),
                (lhs.coordinates * interpolationRate) + (rhs.coordinates * (1 - interpolationRate)),
                (lhs.hardwareTimestamp * interpolationRate) + (rhs.hardwareTimestamp * (1 - interpolationRate)),
                (lhs.speed * interpolationRate) + (rhs.speed * (1 - interpolationRate))
                );
        }
        /// <summary>
        /// Gets a data point at a specific time (automatically interpolated)
        /// </summary>
        /// <param name="time">The time for which to get the datapoint</param>
        /// <returns>The correct data point</returns>
        private PozyxData getDatapointAtTime(float time)
        {
            if (time <= pozyxData[0].systemTimestamp)
                return pozyxData[0];
            for (int i = 0; i < pozyxData.Count - 2; i++)
            {
                // TODO: using systemTimestamp for now, but we migt want to change
                // to hardwareTimestamp later for better accuracy (though that
                // leaves the problem that the calculation for the interpolation
                // rate becomes impossible)
                float minValue = pozyxData[i].systemTimestamp;
                float maxValue = pozyxData[i + 1].systemTimestamp;
                if (minValue == time)
                    return pozyxData[i];
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

        /// <summary>
        /// Clears all the data for the current sensor.
        /// </summary>
        public override void clearData()
        {
            pozyxData.Clear();
            GC.Collect();
        }

        /// <summary>
        /// Gets the speed of the latest data point
        /// </summary>
        /// <returns>The speed of the latest data point</returns>
        public float GetSpeed()
        {
            return pozyxData[pozyxData.Count - 1].speed;
        }

        /// <summary>
        /// Gets the position of the latest data point
        /// </summary>
        /// <returns>The position of the latest data point</returns>
        public Vector3 GetPosition()
        {
            return pozyxData[pozyxData.Count - 1].coordinates;
        }

        /// <summary>
        /// Gets the acceleration (in 3 directions) of the latest data point
        /// </summary>
        /// <returns>The acceleration (in 3 directions) of the latest data point</returns>
        public List<Vector3> GetAccelerometer()
        {
            return pozyxData[pozyxData.Count - 1].accelerometer;
        }

        /// <summary>
        /// Gets the speed of the latest data point
        /// </summary>
        /// <returns>The speed of the datapoint at the given time</returns>
        public float GetSpeed(float timestamp)
        {
            return getDatapointAtTime(timestamp).speed;
        }

        /// <summary>
        /// Gets the position of the datapoint at the given time
        /// </summary>
        /// <returns>The position of the datapoint at the given time</returns>
        public Vector3 GetPosition(float timestamp)
        {
            return getDatapointAtTime(timestamp).coordinates;
        }

        /// <summary>
        /// Gets the acceleration (in 3 directions) at the given time
        /// </summary>
        /// <returns>The acceleration (in 3 directions)  at the given time</returns>
        public List<Vector3> GetAccelerometer(float timestamp)
        {
            return getDatapointAtTime(timestamp).accelerometer;
        }

        public override string ToString()
        {
            return Convert.ToString(streamInstance.GetStreamerID());
        }
    }
}
