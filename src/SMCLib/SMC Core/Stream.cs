using System;
using System.Collections.Generic;

namespace SMC_Core
{
    /// <summary>
    /// Generic stream class shared by all DataType
    /// </summary>
    /// <typeparam name="T">The specific DataType</typeparam>
    public class Stream<T> : IStreamContainer where T : DataEntry
    {
        private readonly int id;
        protected Queue<T> valueStream = new Queue<T>();

        public Type valueType { get; private set; }
        // FIXME ???
        public bool Active { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Stream(int id)
        {
            this.id = id;
            valueType = typeof(T);
        }
        /// <summary>
        /// Reads (and pops) a number of entries from the input data queue
        /// </summary>
        /// <param name="amount">The amount of entries to read</param>
        /// <returns>A number of entries from the recieved data, one entry at a time</returns>
        public IEnumerable<T> Read(int amount)
        {
            for (int i = 0; i < Math.Min(amount, valueStream.Count); i++)
            {
                yield return Read();
            }
        }
        /// <summary>
        /// Pops a single value from the input queue
        /// </summary>
        /// <returns></returns>
        public T Read()
        {
            return valueStream.Dequeue();
        }

        /// <summary>
        /// Writes a single data entry to the input queue
        /// </summary>
        /// <remarks>
        /// Automatically converts from DataEntry to T!
        /// </remarks>
        /// <param name="data"></param>
        public void Write(DataEntry data)
        {
            valueStream.Enqueue(data as T);
        }
        /// <summary>
        /// Writes a range of entries to the input queue
        /// </summary>
        /// <param name="values">The values to write to the input queue</param>
        public void Write(IEnumerable<T> values)
        {
            foreach (T val in values)
            {
                Write(val);
            }
        }
        /// <summary>
        /// Get the number of data entries waiting in the queue
        /// </summary>
        /// <returns>The number of data entries waiting in the queue</returns>
        public int DataAvailable()
        {
            return valueStream.Count;
        }
        /// <summary>
        /// Get the ID of the streamer (which matches the ID of the sensor)
        /// </summary>
        /// <returns></returns>
        // FIXME: @lucas can you double check this? (both code and doc)
        public int GetStreamerID()
        {
            return id;
        }


    }
}
