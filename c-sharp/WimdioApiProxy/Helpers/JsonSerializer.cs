using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.Helpers
{
    public class JsonSerializer : ISerializer
    {
        private JsonSerializerSettings settings => new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            DateFormatString = "yyyy-MM-ddTHH:mm:ss",
            NullValueHandling = NullValueHandling.Ignore,
        };

        public T Deserialize<T>(string input) where T : class
        {
            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        public string Serialize<T>(T input) where T : class
        {
            return JsonConvert.SerializeObject(input, Formatting.Indented, settings);
        }
    }


    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return reader.Value.ToString() == "1";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }

    public class StringObjectConverter<T> : JsonConverter where T : class
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var s = new JsonSerializer();
            var strValue = s.Serialize(value as T);
            writer.WriteValue(strValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var strValue = reader.Value.ToString();
            var s = new JsonSerializer();
            return s.Deserialize<T>(strValue);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}
