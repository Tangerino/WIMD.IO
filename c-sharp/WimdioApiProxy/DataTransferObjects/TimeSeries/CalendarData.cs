using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.TimeSeries
{
    public class CalendarData
    {
        [JsonProperty("t")]
        public DateTime Date { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }
}
