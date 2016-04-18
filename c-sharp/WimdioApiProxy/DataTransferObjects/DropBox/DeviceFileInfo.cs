using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.DropBox
{
    public class DeviceFileInfo : NewFile
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
    }
}
