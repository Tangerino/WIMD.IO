using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Calendars
{
    public class SpecialDay
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("day")]
        public DateTime Day { get; set; }

        [JsonProperty("recurrent")]
        public bool IsRecurrent { get; set; }
    }
}
