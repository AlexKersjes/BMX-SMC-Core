using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SMC_Core
{
    public static partial class Parsers
    {

        //TODO: Move the parse functions somewhere else
        /// <summary>
        /// Automatically (tries to) parse data from JSON to the given data type
        /// </summary>
        /// <typeparam name="T">Data entry subclass</typeparam>
        /// <param name="json">Input JSON to parse</param>
        /// <returns></returns>
        public static IEnumerable<T> ParseJson<T>(string json) where T : DataEntry
        {
            yield return JsonConvert.DeserializeObject<T>(json);
        }

        // If the Newtonsoft parser is not good enough for your data type, override the above function like this:
        // public Array<PozyxData> parseJSON(string json)
        // {
        //     return new Array<PozyxData>();
        // }

    }
}
