using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC_Core;

namespace SMCUnitTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SensorHandlerEdgeCaseTests
    {
        public SensorHandlerEdgeCaseTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestMethod]
        public void TestAddDataThrowsNotImplementedExceptionOnUnknownDataType()
        {
            StreamHandler handler = new StreamHandler();
            SensorHandler sensorHandler = new SensorHandler();
            handler.AddSensorHandlerCallback(StubSensorCallbackAddsensorThrowsException);
            var StubData = new EmptyStubData(DateTime.Now.Millisecond, 5);
            handler.AddData<EmptyStubData>(StubData);
        }

        private int StubSensorCallbackAddsensorThrowsException(IStreamContainer container)
        {
            SensorHandler sensorHandler = new SensorHandler();
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                sensorHandler.addSensor(container);
            });
            return 0;
        }
    }
}
