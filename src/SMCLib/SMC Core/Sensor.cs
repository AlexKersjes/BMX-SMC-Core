namespace SMC_Core
{
    /// <summary>
    /// Generic sensor class that all sensors inherit from
    /// </summary>
    public abstract class Sensor
    {
        int id;
        protected Sensor(int id)
        {
            this.id = id;
        }

        public virtual int GetID()
        {
            return id;
        }

        // TODO: do we want to do anything in the getter/setter?
        /// <summary>
        /// Whether or not to record the incoming data
        /// </summary>
        public bool Active { get => active; set => active = value; }
        /// <summary>
        /// Clears all the data for the current sensor.
        /// </summary>
        public abstract void clearData();
        /// <summary>
        /// Retrieves data from the streamHandler and handles data calibration
        /// </summary>
        public abstract void updateData();
        protected bool active;
    }
}
