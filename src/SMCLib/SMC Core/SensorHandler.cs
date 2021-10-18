using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC_Core
{
    // TODO: SensorHandler looks like a static class. Maybe refactor it to be made by a factory?
    /// <summary>
    /// Sensor handler class to distribute incoming data entry to the correct sensor
    /// </summary>
    public class SensorHandler
    {
        private List<Sensor> Sensors = new List<Sensor>();

        // public void Test()
        // {
        //     foreach (PozyxSensor Sensor in getSensorsofType<PozyxSensor>())
        //     {
        //     }
        // }

        /// <summary>
        /// Gets all sensors of a certain type
        /// </summary>
        /// <typeparam name="T">Sensor type</typeparam>
        /// <returns></returns>
        public IEnumerable<T> getSensorsofType<T>() where T : Sensor
        {
            foreach (Sensor Sensor in Sensors)
            {
                //get stream container type
                if (Sensor.GetType() == typeof(T))
                {
                    yield return Sensor as T;
                }
            }
        }

        /// <summary>
        /// Generates a new sensor (or re-uses an old one) with a certain stream
        /// </summary>
        /// <param name="stream">The stream from which the sensor should get data</param>
        /// <returns>The ID of the newly added sensor</returns>
        public int addSensor(IStreamContainer stream)
        {
            Sensor Sensor;

            // TODO: Check if sensor already exists, and add stream to it (?)
            if (stream.valueType.Equals(typeof(PozyxData)))
            {
                // TODO: Add string id name here
                Sensor = new PozyxSensor(stream.GetStreamerID(), (Stream<PozyxData>)stream);
                Sensor.Active = true;
            }
            else
            {
                // TODO: Add custom exception here
                throw new NotImplementedException();
            }
            Sensors.Add(Sensor);
            return Sensor.GetID();
        }
        /// <summary>
        /// Removes all sensors with a certain ID
        /// </summary>
        /// <param name="id">The ID of the sensor to remove</param>
        public void removeSensor(int id)
        {
            Sensors.RemoveAll(s => s.GetID() == id);
        }
        /// <summary>
        /// Removes a certain sensor
        /// </summary>
        /// <param name="s">The sensor to be removed</param>
        public void removeSensor(Sensor s)
        {
            Sensors.Remove(s);
        }
        /// <summary>
        /// Gets a sensor by ID
        /// </summary>
        /// <param name="id">The ID of the sensor to be gotten</param>
        /// <returns>The sensor</returns>
        public Sensor getSensor(int id)
        {
            return Sensors.Single(s => s.GetID() == id);
        }
        /// <summary>
        /// Updates the data for all sensors (whithin the sensorhandler)
        /// </summary>
        public void updateData()
        {
            Sensors.ForEach(s => s.updateData());
        }

        public List<Sensor> GetSensors() { return Sensors; }
    }
}
