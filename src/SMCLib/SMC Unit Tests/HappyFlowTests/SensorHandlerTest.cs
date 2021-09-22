using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC_Core;

namespace SMCUnitTests
{
    [TestClass]
    public class SensorHandlerTest
    {
        public SensorHandlerTest()
        {
        }

        private TestContext testContextInstance;

        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        public TestContext TestContext
        {
            get => testContextInstance;
            set => testContextInstance = value;
        }

        [TestMethod]
        public void TestCreateStreamHandlerAndCheckIfCallbackIsCorrect()
        {
            StreamHandler handler = new StreamHandler();
            handler.AddSensorHandlerCallback(StubSensorCallback);
            var StubData = new EmptyStubData(0, 5);
            handler.AddData<EmptyStubData>(StubData);
        }

        private int StubSensorCallback(IStreamContainer container)
        {
            Assert.AreEqual(5, container.GetStreamerID());
            return 0;
        }

        [TestMethod]
        public void TestCreateStreamHandler()
        {
        }
    }
}
