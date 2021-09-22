using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC_Core;

namespace SMCUnitTests
{
    [TestClass]
    public class PozyxDataJsonParserTest
    {
        public PozyxDataJsonParserTest()
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
        public void TestPozyxDataParser()
        {
            
            foreach (PozyxData data in Parsers.ParseJson(generateDataEntry())) 
            {
                //TODO: Add asserts here to check if json values are correct in PozyxData Class
            }
        }



        private string generateDataEntry() 
        {
            //TODO: Randomize values?
            return "[{\n    \"version\": \"2.0\",\n    \"tagId\": \"0\",\n    \"timestamp\": 1621588653.7813396,\n    \"data\": {\n        \"coordinates\": {\n            \"x\": 93,\n            \"y\": 93,\n            \"z\": 97\n        },\n        \"velocity\": {\n            \"x\": 13,\n            \"y\": 78,\n            \"z\": 87\n        },\n        \"acceleration\": {\n            \"x\": 96,\n            \"y\": 9,\n            \"z\": 30\n        },\n\n        \"tagData\": {\n            \"blinkIndex\": 226,\n            \"accelerometer\": [\n                [70, 427, 262]\n            ]\n        },\n    },\n    \"success\": true\n    }]\n    ";
        }
    }
}
