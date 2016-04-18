using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Places
{
    public class Place : NewPlace
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
