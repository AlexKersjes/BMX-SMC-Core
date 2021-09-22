using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC_Core;

namespace SMCUnitTests
{
    [TestClass]
    public class PozyxSensorTest
    {
        public PozyxSensorTest()
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
        public void TestDataInterpolation()
        {
            PozyxData lhs = new PozyxData(0, 0, 0, new List<Vector3>() { new Vector3(0, 0, 0) }, new Vector3(0, 0, 0), 0, 0);
            PozyxData rhs = new PozyxData(2, 0, 2, new List<Vector3>() { new Vector3(2, 2, 2) }, new Vector3(2, 2, 2), 2, 2);
            PozyxData expectedResult = new PozyxData(1, 0, 1, new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(2, 2, 2) }, new Vector3(1, 1, 1), 1, 1);
            PozyxData result = StubPozyxSensor.Interpolate(lhs, rhs, 0.5f);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
