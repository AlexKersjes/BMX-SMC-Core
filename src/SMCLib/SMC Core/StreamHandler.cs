using System.Collections.Generic;
using System.Linq;

namespace SMC_Core
{
    public class StreamHandler : IStreamProvider
    {

        public delegate int SensorHandlerFunc(IStreamContainer container);
        private event SensorHandlerFunc SensorCreatedCallback;

        public List<IStreamContainer> streamContainers = new List<IStreamContainer>();
        /// <summary>
        /// Gets all streamers
        /// </summary>
        /// <returns>All streamers</returns>
        public IEnumerable<IStreamContainer> getAvailableStreamers()
        {
            foreach (IStreamContainer streamContainer in streamContainers)
            {
                yield return streamContainer;
            }
        }
        /// <summary>
        /// Gets a streamer by its streamer ID
        /// </summary>
        /// <param name="id">the streamer ID</param>
        /// <returns>A streamer</returns>
        public IStreamContainer getStreamByID(int id)
        {
            return streamContainers.SingleOrDefault(s => s.GetStreamerID() == id);
        }
        /// <summary>
        /// Adds a stream with a data entry
        /// </summary>
        /// <typeparam name="T">The DataType</typeparam>
        /// <param name="data">The data to be added to the newly created stream</param>
        /// <returns>The newly created stream (with the correct data)</returns>
        private IStreamContainer CreateStreamWithData<T>(T data) where T : DataEntry
        {
            Stream<T> newStream = new Stream<T>(data.id);
            streamContainers.Add(newStream);
            return newStream;
            //TODO: When stream is created, SensorHandler should receive stream without use of the current callback
        }

        public void RemoveStreamByID(int id)
        {
            IStreamContainer stream = getStreamByID(id);
            streamContainers.Remove(stream);
        }

        /// <summary>
        /// Adds a data entry to the correct stream (and automatically makes a new stream if needed).
        /// </summary>
        /// <typeparam name="T">The DataType</typeparam>
        /// <param name="data">The data to be added</param>
        public void AddData<T>(T data) where T : DataEntry
        {
            IStreamContainer stream = getStreamByID(data.id);
            if (stream == null)
            {
                stream = CreateStreamWithData(data);
                // TODO: Refactor this
                SensorCreatedCallback?.Invoke(stream);
            }
            stream.Write(data);

        }
        /// <summary>
        /// Adds a range of data entries (sorting is done automatically)
        /// </summary>
        /// <typeparam name="T">The DataType</typeparam>
        /// <param name="data">The data to be added</param>
        public void AddData<T>(IEnumerable<T> data) where T : DataEntry
        {
            foreach (T entry in data)
            {
                AddData(entry);
            }
        }
        /// <summary>
        /// Add a callback that gets called when a new sensor is added
        /// </summary>
        /// <param name="callback">The callback to be added</param>
        public void AddSensorHandlerCallback(SensorHandlerFunc callback)
        {
            SensorCreatedCallback += callback;
        }
    }
}
