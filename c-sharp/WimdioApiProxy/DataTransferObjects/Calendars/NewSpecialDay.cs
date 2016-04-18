using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Calendars
{
    public class NewSpecialDay
    {
        public NewSpecialDay() { }
        public NewSpecialDay(SpecialDay specialDay)
        {
            Name = specialDay.Name;
            Day = specialDay.Day;
            IsRecurrent = specialDay.IsRecurrent;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("day")]
        public DateTime Day { get; set; }

        [JsonProperty("recurrent")]
        public bool IsRecurrent { get; set; }
    }
}
