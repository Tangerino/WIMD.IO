using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Calendars
{
    public class NewCalendar
    {
        public NewCalendar() { } 
        public NewCalendar(Calendar calendar)
        {
            Name = calendar.Name;
        }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
