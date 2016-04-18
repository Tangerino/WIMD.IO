using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Calendars
{
    public class NewPeriod
    {
        public NewPeriod() { }
        public NewPeriod(Period period)
        {
            Name = period.Name;
            StartTime = period.StartTime;
            EndTime = period.EndTime;
            Monday = period.Monday;
            Tuesday = period.Tuesday;
            Wednesday = period.Wednesday;
            Thursday = period.Thursday;
            Friday = period.Friday;
            Saturday = period.Saturday;
            Sunday = period.Sunday;
            Special = period.Special;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("start_time")]
        public TimeSpan StartTime { get; set; }

        [JsonProperty("end_time")]
        public TimeSpan EndTime { get; set; }

        [JsonProperty("mon")]
        public bool Monday { get; set; }

        [JsonProperty("tue")]
        public bool Tuesday { get; set; }

        [JsonProperty("wed")]
        public bool Wednesday { get; set; }

        [JsonProperty("thu")]
        public bool Thursday { get; set; }

        [JsonProperty("fri")]
        public bool Friday { get; set; }

        [JsonProperty("sat")]
        public bool Saturday { get; set; }

        [JsonProperty("sun")]
        public bool Sunday { get; set; }

        [JsonProperty("spc")]
        public bool Special { get; set; }
    }
}
