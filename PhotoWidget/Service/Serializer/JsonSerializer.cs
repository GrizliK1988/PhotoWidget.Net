using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace PhotoWidget.Service.Serializer
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        public string Serialize(T obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, obj);

            memoryStream.Position = 0;

            var streamReader = new StreamReader(memoryStream);
            var serialized = streamReader.ReadToEnd();

            streamReader.Close();
            memoryStream.Close();

            return serialized;
        }

        public T Deserialize(string data)
        {
            throw new NotImplementedException();
        }
    }
}