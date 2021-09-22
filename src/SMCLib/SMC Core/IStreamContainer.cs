using System;

namespace SMC_Core
{
    /// <summary>
    /// Generic interface for containing streams.
    /// </summary>
    /// <remarks>
    /// This is needed to be able to have a list of 
    /// multiple Streams with differing data types
    /// </remarks>
    public interface IStreamContainer
    {
        /// <summary>
        /// Whether or not to record the incoming data
        /// </summary>
        bool Active { get; set; }
        /// <summary>
        /// Gets the streamer ID
        /// </summary>
        /// <returns>The streamer ID</returns>
        int GetStreamerID();
        /// <summary>
        /// The type of the stream contained within
        /// </summary>
        Type valueType { get; }
        /// <summary>
        /// Writes a data entry into the stream
        /// </summary>
        /// <param name="data">The data entry to write</param>
        void Write(DataEntry data);
    }
}
