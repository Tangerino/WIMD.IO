using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class Sensor : NewSensor
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
