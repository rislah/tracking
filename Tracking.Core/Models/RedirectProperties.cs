using Newtonsoft.Json;

namespace Tracking.Core.Models
{
    public class RedirectProperties
    {
        [JsonProperty(PropertyName = "event_name")]
        public string EventName { get; set; }

        [JsonProperty(PropertyName = "ip")] public string Ip { get; set; }

        [JsonProperty(PropertyName = "target_url")]
        public string TargetUrl { get; set; }

        public override string ToString()
        {
            return $"{nameof(EventName)}: {EventName}, {nameof(Ip)}: {Ip}, {nameof(TargetUrl)}: {TargetUrl}";
        }
    }
}