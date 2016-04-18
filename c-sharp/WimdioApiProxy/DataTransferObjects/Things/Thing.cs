using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Things
{
    public class Thing : NewThing
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
