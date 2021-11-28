using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tracking.Core.Interfaces;
using Tracking.Core.Models;
using Tracking.Core.Validators;

namespace Tracking.Core.Services
{
    public class RedirectLinkService : IRedirectLinkService
    {
        private readonly HMACSHA1 _hmacSha = new(Encoding.ASCII.GetBytes(Constants.HmacSecret));
        private readonly ILogger<RedirectLinkService> _logger;
        private readonly IMessaging _messaging;

        public RedirectLinkService(IMessaging messaging, ILogger<RedirectLinkService> logger)
        {
            _messaging = messaging;
            _logger = logger;
        }

        public async Task<string> Redirect(ILink link, RedirectParams redirectParams, CancellationToken cancellationToken)
        {
            try
            {
                var validHmac = await link.VerifyHmac(_hmacSha, redirectParams.Hmac, cancellationToken);
                if (!validHmac)
                {
                    _logger.LogInformation("invalid hmac");
                    return Constants.FallbackRedirectUrl;
                }

                var propertiesBytes = DeserializeRedirectProperties(redirectParams, out var properties);
                
                var validator = new RedirectPropertiesValidator();
                var validationResult = await validator.ValidateAsync(properties, cancellationToken);
                if (!validationResult.IsValid)
                {
                    _logger.LogInformation("invalid properties: {}", validationResult.Errors);
                    return Constants.FallbackRedirectUrl;
                }

                var topic = EventTopicStore.GetTopic(properties.EventName);
                var message = new Message(topic, propertiesBytes);
                
                _messaging.Publish(message);

                return properties.TargetUrl;
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogInformation("invalid event");
                return Constants.FallbackRedirectUrl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Constants.FallbackRedirectUrl;
            }
        }

        private static byte[] DeserializeRedirectProperties(RedirectParams redirectParams,
            out RedirectProperties properties)
        {
            var rawBytes = Convert.FromBase64String(redirectParams.PropertiesBase64);
            var rawPropertiesJson = Encoding.UTF8.GetString(rawBytes);
            properties = JsonConvert.DeserializeObject(rawPropertiesJson, typeof(RedirectProperties)) as RedirectProperties;
            return rawBytes;
        }
    }
}