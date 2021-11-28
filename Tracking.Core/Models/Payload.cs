using Newtonsoft.Json;

namespace Tracking.Core.Models
{
    public class Payload
    {
        [JsonProperty("event")] public string Event { get; set; }
        [JsonProperty("properties")] public object Properties { get; set; }
    }
}