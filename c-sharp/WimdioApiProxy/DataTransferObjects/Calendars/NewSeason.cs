using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Calendars
{
    public class NewSeason
    {
        public NewSeason() { }
        public NewSeason(Season season)
        {
            Name = season.Name;
            StartDate = season.StartDate;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
    }
}
