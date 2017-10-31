using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Serializer
{
    public class Serializer : ISerializer
    {
        private readonly JsonSerializer serializer;

        public Serializer(IContractResolver contractResolver)
            : this()
        {
            serializer.ContractResolver = contractResolver;
        }

        public Serializer()
        {
            serializer = new JsonSerializer();
        }

        public string Serialize(object obj)
        {
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.GetStringBuilder().ToString();
            }
        }

        public T Deserialize<T>(string data)
        {
            using (var reader = new StringReader(data))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}
