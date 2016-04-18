using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class ShadowObjectContent
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("enabled")]
        [JsonConverter(typeof(BoolConverter))]
        public bool? Enabled { get; set; }

        [JsonProperty("host")]
        public int? Host { get; set; }

        [JsonProperty("publishinterval")]
        public int? PublishInterval { get; set; }

        [JsonProperty("tagposition")]
        public int? TagPosition { get; set; }

        [JsonProperty("lastrun")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? LastRun { get; set; }

        [JsonProperty("nextrun")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? NextRun { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("pause")]
        [JsonConverter(typeof(BoolConverter))]
        public bool? Pause { get; set; }

        [JsonProperty("zipit")]
        [JsonConverter(typeof(BoolConverter))]
        public bool? ZipIt { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("cleanSession")]
        [JsonConverter(typeof(BoolConverter))]
        public bool? CleanSession { get; set; }

        [JsonProperty("timeout")]
        public int? Timeout { get; set; }

        [JsonProperty("activationcode")]
        public Guid? ActivationCode { get; set; }

        [JsonProperty("feedid")]
        public Guid? FeedId { get; set; }

        [JsonProperty("mailto")]
        public string MailTo { get; set; }

        [JsonProperty("mailcc")]
        public string MailCc { get; set; }

        [JsonProperty("mailbcc")]
        public string MailBcc { get; set; }

        [JsonProperty("eventposition")]
        public int? EventPosition { get; set; }

        [JsonProperty("alarmposition")]
        public int? AlarmPosition { get; set; }

        [JsonProperty("configposition")]
        public int? ConfigPosition { get; set; }
    }
}
