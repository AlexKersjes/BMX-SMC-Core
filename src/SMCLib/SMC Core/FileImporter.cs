using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace SMC_Core
{
    /// <summary>
    /// File importer class. Imports files from different file formats (JSON and CSV for now)
    /// </summary>
    public class FileImporter
    {
        // Not needed anymore since we will just send raw DataEntry instances to StreamHandler
        //private Stack<IStreamContainer> streamContainers;
        /// <summary>
        /// Constructor for the FileImporter class. 
        /// This class is constructed, used and desctructed almost immediately
        /// </summary>
        /// <param name="path">Path to the file to be imported</param>
        /// <param name="handler">Handler to which to send data</param>
        public FileImporter(string path, StreamHandler handler)
        {
            StreamReader reader = new StreamReader(path);

            string s = reader.ReadToEnd();
            IEnumerable<PozyxData> data;
            switch (Path.GetExtension(path))
            {
                // json
                case ".a51":
                    // TODO: only handles PozyxData for now. 
                    data = Parsers.ParseJson<PozyxData>(s);
                    handler.AddData<PozyxData>(data);
                    break;
                // csv
                case ".csv":
                    // TODO: only handles PozyxData for now.
                    data = Parsers.ParseCSV<PozyxData>(s);
                    handler.AddData<PozyxData>(data);
                    break;
                default:
                    // *should* never happen
                    throw new NotImplementedException();
            }
        }

       

       
    }
}

