using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WimdioApiProxy.v2.DataTransferObjects.Etls
{
    public class Etl : NewEtl
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("lastrun")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime LastRun { get; set; }
    }
}
