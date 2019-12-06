using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ScoopFramework.TTGenerators.Helper
{
    public static class Json
    {
        public static string Serialize<T>(T instance)
        {
            return JsonConvert.SerializeObject(instance);
            //var serializer = new DataContractJsonSerializer(typeof(T));
            //using (var memoryStream = new MemoryStream())
            //{
            //    serializer.WriteObject(memoryStream, instance);
            //
            //    memoryStream.Flush();
            //    memoryStream.Position = 0;
            //
            //    using (var reader = new StreamReader(memoryStream))
            //    {
            //        return reader.ReadToEnd();
            //    }
            //}
        }

        public static object DeserializeObject(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }

        public static T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
            //if (string.IsNullOrEmpty((serialized)))
            //    return default(T);
            ////  return (T)Activator.CreateInstance(typeof (T));
            //var serializer = new DataContractJsonSerializer(typeof(T));

            //using (var memoryStream = new MemoryStream())
            //{
            //    using (var writer = new StreamWriter(memoryStream))
            //    {
            //        writer.Write(serialized);
            //        writer.Flush();

            //        memoryStream.Position = 0;

            //        return (T)serializer.ReadObject(memoryStream);
            //    }
            //}
        }
        public static T Deserialize2<T>(string serialized)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    writer.Write(serialized);
                    writer.Flush();

                    ms.Position = 0;

                    XmlSerializer deserializer = new XmlSerializer(typeof(T));
                    return (T)deserializer.Deserialize(ms);
                }

            }
        }
    }

    public class JsonHelper
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, t);
            var jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            var obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }
}