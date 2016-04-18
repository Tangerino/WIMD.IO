using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class CommandState
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("error")]
        public int? Error { get; set; }
    }
}
