using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.DropBox
{
    public class FileInfo : DeviceFileInfo
    {
        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "sent")]
        public DateTime Sent { get; set; }

        [JsonProperty(PropertyName = "executed")]
        public DateTime Executed { get; set; }

        [JsonProperty(PropertyName = "status")]
        public Status Status { get; set; }
    }

    public enum Status
    {
        DOWNLOAD_ERROR,
        RECEIVED,
        EXECUTION_ERROR,
        EXECUTED
    }
}
