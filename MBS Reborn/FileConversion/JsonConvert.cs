using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MBS_Reborn.FileConversion
{
    internal class JsonConvert
    {
        public static T DeserializeFromFile<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static void SerializeToFile<T>(T obj, string filePath)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            File.WriteAllText(filePath, json);
        }
    }
}
