using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC_Core;

namespace SMCUnitTests
{
    [TestClass]
    public class StreamHandlerTest
    {
        [TestMethod]
        public void TestAddingNewStreamer()
        {
            PozyxData data = new PozyxData(0, 0, 0, new List<Vector3>() { Vector3.Zero }, Vector3.Zero, 0, 100)
            {
                id = 50
            };
            StreamHandler handler = new StreamHandler();
            // this adds new streamers, since adding data to a non existing stream creates the stream
            handler.AddData<PozyxData>(data);
            IStreamContainer streamer = handler.getStreamByID(50);
            Assert.IsNotNull(streamer);
            Assert.AreEqual(50, streamer.GetStreamerID());
        }
        [TestMethod]
        public void TestAddToStreamerAndRead()
        {
            StreamHandler handler = new StreamHandler();
            handler.AddData<EmptyStubData>(new EmptyStubData(0, 0));
        }
    }
}
