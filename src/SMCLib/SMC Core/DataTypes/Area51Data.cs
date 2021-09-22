namespace SMC_Core
{
    public class Area51Data : DataEntry
    {
        /// <summary>
        /// Dummy measurement parameter
        /// </summary>
        public float measurementvalue;

        /// <summary>
        /// Dummy class for holding data
        /// </summary>
        /// <param name="timestamp">Timestamp at which the data was recieved</param>
        /// <param name="id">Sensor ID</param>
        /// <param name="measurementvalue">Dummy parameter</param>

        public Area51Data(int timestamp, int id, float measurementvalue)
            : base(timestamp, id)
        {
            this.measurementvalue = measurementvalue;
        }
    }
}