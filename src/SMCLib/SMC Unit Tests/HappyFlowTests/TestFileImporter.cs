using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC_Core;

namespace SMCUnitTests
{
    [TestClass]
    public class TestFileImporter
    {
        private static string GetTempFilePathWithExtension(string extension)
        {
            string path = Path.GetTempPath();
            string fileName = Guid.NewGuid().ToString() + "." + extension;
            return Path.Combine(path, fileName);
        }

        public string CreateRandomFileWithContent(string extension, string data)
        {
            string fileName = GetTempFilePathWithExtension(extension);
            using (StreamWriter w = File.AppendText(fileName))
            {
                w.WriteLine(data);
            }
            return fileName;
        }

        [TestMethod]
        public void aTestFileImporter()
        {
            StreamHandler handler = new StreamHandler();
            string filename = CreateRandomFileWithContent(".a51", @"{'timestamp': '0', 'id': '1', 'test1Param': '1', 'test2Param': 'test', 'test3Param': 'true'}");
            FileImporter importer = new FileImporter(filename, handler);
            foreach (var stream in handler.getAvailableStreamers())
            {
                var castedStream = ((Stream<PozyxData>)stream);
                int amountToRead = castedStream.DataAvailable();
                foreach (var data in castedStream.Read(amountToRead))
                {
                }
            }
        }
        [TestMethod]
        public void testParseCSV()
        {
            string filename = CreateRandomFileWithContent("csv",
                "timestamp,id,test1Param,test2Param,test3Param\n" +
                "50,100,1,2,true\n");
            foreach (StubCsvData testData in Parsers.ParseCSV<StubCsvData>(filename))
            {
                Assert.AreEqual(1, testData.test1Param);
                Assert.AreEqual("2", testData.test2Param);
                Assert.AreEqual(true, testData.test3Param);
            }
        }

        [TestMethod]
        public void testParseJSON()
        {
            string StubJsonFile = @"{'timestamp': '0', 'id': '1', 'test1Param': '1', 'test2Param': 'test', 'test3Param': 'true'}";
            foreach (var testData in Parsers.ParseJson<StubJsonData>(StubJsonFile))
            {
                Assert.AreEqual(0, testData.systemTimestamp);
                Assert.AreEqual(1, testData.id);
                Assert.AreEqual(1, testData.test1Param);
                Assert.AreEqual("test", testData.test2Param);
                Assert.AreEqual(true, testData.test3Param);
            }
        }
    }
}
