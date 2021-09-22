namespace SMCLib
{
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

        public bool Active { get => active; set => active = value; } // TODO: do we want to do anything?
        public abstract void clearData();
        public abstract void updateData();
        protected bool active;
    }
}
