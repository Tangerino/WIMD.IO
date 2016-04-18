using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Calendars
{
    public class Calendar
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
