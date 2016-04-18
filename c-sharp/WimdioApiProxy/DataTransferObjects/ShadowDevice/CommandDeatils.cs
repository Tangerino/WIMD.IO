using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class CommandDeatils : NewCommand
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("sent")]
        public DateTime Sent { get; set; }

        [JsonProperty("acknowledge")]
        public DateTime? Acknowledge { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("createdby")]
        public Guid CreatedBy { get; set; }

        [JsonProperty("duedate")]
        public DateTime DueDate { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("deletedby")]
        public Guid? DeletedBy { get; set; }
    }
}
