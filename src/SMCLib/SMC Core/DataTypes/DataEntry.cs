namespace SMC_Core
{
    public class DataEntry
    {
        // Data attributes shared between all data types go here
        /// <summary>
        /// Timestamp at which the data was recieved
        /// </summary>
        public long systemTimestamp;
        /// <summary>
        /// Sensor ID
        /// </summary>
        public int id;
        /// <summary>
        /// Generic contructor shared by all measurement data types
        /// </summary>
        /// <param name="timestamp">Timestamp at which the data was recieved</param>
        /// <param name="id">Sensor ID</param>
        public DataEntry(long timestamp, int id)
        {
            systemTimestamp = timestamp;
            this.id = id;
        }

        /// <summary>
        /// Provides a string representation of a generic data entry
        /// </summary>
        /// <returns>A string representation of a generic data entry</returns>
        public override string ToString()
        {
            return "System timestamp: " + systemTimestamp.ToString();
        }
    }
}
