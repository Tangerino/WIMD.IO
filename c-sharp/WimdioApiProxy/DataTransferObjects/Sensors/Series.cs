using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class SensorData
    {
        public SensorData()
        {
            Series = new List<SensorSerie>();
        }

        [JsonProperty("series")]
        public List<SensorSerie> Series { get; set; }
    }

    public class SensorSerie
    {
        public SensorSerie()
        {
            Values = new List<object[]>();
        }

        [JsonProperty("id")]
        public string RemoteId { get; set; }

        [JsonProperty("values")]
        public List<object[]> Values { get; set; }

        public void AddValue(DateTime timestamp, double value)
        {
            Values.Add(new object[] { timestamp, value });
        }
    }

    public class SerieValue
    {
        [JsonProperty(PropertyName = null)]
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = null)]
        public decimal Value { get; set; }
    }
}
