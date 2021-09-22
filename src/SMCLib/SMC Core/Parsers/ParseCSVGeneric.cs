using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;



namespace SMC_Core
{
    public static partial class Parsers
    {

        /// <summary>
        /// Automatically (tries to) parse data from CSV to the given data type
        /// </summary>
        /// <typeparam name="T">Data entry subclass</typeparam>
        /// <param name="csv">Input CSV to parse</param>
        /// <returns>All records that were succesfully parsed, a singular record at a time</returns>
        public static IEnumerable<T> ParseCSV<T>(string csv) where T : DataEntry
        {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using (TextReader reader = new StreamReader(csv))
            using (CsvReader csvReader = new CsvReader(reader, config))
            {
                foreach (T record in csvReader.GetRecords<T>())
                {
                    yield return record;
                }
            }
        }

    }
}
