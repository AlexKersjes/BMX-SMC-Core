using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SMC_Core;

namespace SMCUnitTests.HappyFlowTests
{
    /// <summary>
    /// Summary description for JsonPathConverterTest
    /// </summary>
    [TestClass]
    public class JsonPathConverterTest
    {

        [JsonConverter(typeof(JsonPathConverter))]
        class Person
        {
            public string Name;
            public int Age { get; set; }

            [JsonProperty("picture.data.url")]
            public string ProfilePicture { get; set; }

            [JsonProperty("favorites.movie")]
            public Movie FavoriteMovie { get; set; }

            [JsonProperty("favorites.color")]
            public string FavoriteColor { get; set; }
        }

        [Serializable]
        class Movie
        {
            public string Title { get; set; }
            public int Year { get; set; }

            public int testValue;
        }


        public JsonPathConverterTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get => testContextInstance;
            set => testContextInstance = value;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestCustomClassDeserialize()
        {
            string json = @"
            {
                ""Name"" : ""Joe Shmoe"",
                ""Age"" : 26,
                ""picture"":
                {
                    ""id"": 123456,
                    ""data"":
                    {
                        ""type"": ""jpg"",
                        ""url"": ""http://www.someplace.com/mypicture.jpg""
                    }
                },
                ""favorites"":
                {
                    ""movie"": 
                    {
                        ""title"": ""The Godfather"",
                        ""starring"": ""Marlon Brando"",
                        ""year"": 1972
                    },
                    ""color"": ""purple"",
                }
            }";
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            serializerSettings.Converters.Add(new JsonPathConverter());
            Person p = JsonConvert.DeserializeObject<Person>(json, serializerSettings);

            Assert.AreEqual("Joe Shmoe", p.Name);
            Assert.AreEqual(26, p.Age);
            Assert.AreEqual("http://www.someplace.com/mypicture.jpg", p.ProfilePicture);
            Assert.AreEqual("The Godfather", p.FavoriteMovie.Title);
            Assert.AreEqual("purple", p.FavoriteColor);
            p.FavoriteMovie.testValue = 10;

            string newJson = JsonConvert.SerializeObject(p, serializerSettings);


        }
    }
}
