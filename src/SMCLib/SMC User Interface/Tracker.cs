namespace SMCLib
{
    public abstract class Sensor
    {
        protected int id;
        public virtual int GetID()
        {
            return id;
        }
        public abstract void clearData();
        protected abstract void updateData();
    }
}
