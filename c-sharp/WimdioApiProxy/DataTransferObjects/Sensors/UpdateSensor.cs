using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class UpdateSensor
    {
        public UpdateSensor(Sensor sensor)
        {
            Name = sensor.Name;
            Description = sensor.Description;
            Unit = sensor.Unit;
            Tseoi = sensor.Tseoi;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("tseoi")]
        public int Tseoi { get; set; }
    }
}
