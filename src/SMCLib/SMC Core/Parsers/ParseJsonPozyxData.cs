using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
namespace SMC_Core
{
    public static partial class Parsers
    {


        public static IEnumerable<PozyxData> ParseJson(string json) 
        {

            // get data property from json 
            JArray arr = JArray.Parse(json);


            //  foreach (JObject dataObj in obj.DescendantsAndSelf().OfType<JObject>())
            // {

            //JObject dataObj = (JObject)obj.First;
            JObject dataObj;
            dataObj = (JObject)arr.First;
            JObject child = dataObj.Value<JObject>("data");


            JObject tagData = child.Value<JObject>("tagData");
            int id = dataObj.Value<int>("tagId");
            int hardwareTimestamp = dataObj.Value<int>("timestamp");
            float score = dataObj.Value<float>("score");

            Vector3 coordinates = new Vector3();
            coordinates.X = (float)child["coordinates"]["x"];
            coordinates.Y = (float)child["coordinates"]["y"];
            coordinates.Z = (float)child["coordinates"]["z"];



            List<Vector3> acceleration = new List<Vector3>();

            //FIXME: one acceleration value, should parse array of accelration

            Vector3 accelValue = new Vector3();

            accelValue.X = (float)child["coordinates"]["x"];
            accelValue.Y = (float)child["coordinates"]["y"];
            accelValue.Z = (float)child["coordinates"]["z"];


            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


            PozyxData data = new PozyxData(timestamp, id, score, acceleration, coordinates, hardwareTimestamp, 0);

            yield return data;

        }

     

    }
}
