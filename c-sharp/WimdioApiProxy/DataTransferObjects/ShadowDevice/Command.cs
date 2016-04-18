using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class Command : NewCommand
    {
        [JsonProperty("commandid")]
        public Guid Id { get; set; }

        [JsonProperty("duedate")]
        public DateTime DueDate { get; set; }
    }
}
