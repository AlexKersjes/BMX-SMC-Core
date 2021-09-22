using System.Collections.Generic;

namespace SMC_Core
{
    /// <summary>
    /// Stream provider interfaces (for classes that can provide streamers)
    /// </summary>
    public interface IStreamProvider
    {
        IEnumerable<IStreamContainer> getAvailableStreamers();
    }
}
