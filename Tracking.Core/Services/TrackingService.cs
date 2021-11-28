using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tracking.Core.Interfaces;
using Tracking.Core.Models;

namespace Tracking.Core.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IMessaging _messaging;
        private readonly ILogger<TrackingService> _logger;

        public TrackingService(IMessaging messaging, ILogger<TrackingService> logger)
        {
            _messaging = messaging;
            _logger = logger;
        }

        public void Track(Payload payload)
        {
            try
            {
                var properties = JsonConvert.SerializeObject(payload.Properties);
                var topic = EventTopicStore.GetTopic(payload.Event);
                var message = new Message(topic, Encoding.UTF8.GetBytes(properties));
                _messaging.Publish(message);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation("unknown event {}", payload.Event);
            }
        }
    }
}