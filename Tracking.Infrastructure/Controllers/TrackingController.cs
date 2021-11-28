using System;
using System.Text;
using System.Threading;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tracking.Core;
using Tracking.Core.Models;
using Tracking.Core.Services;

namespace Tracking.Infrastructure.Controllers
{
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ILogger<TrackingController> _logger;
        private readonly IRedirectLinkService _redirectLinkService;
        private readonly ITrackingService _trackingService;

        public TrackingController(ILogger<TrackingController> logger, IRedirectLinkService redirectLinkService, ITrackingService trackingService)
        {
            _logger = logger;
            _redirectLinkService = redirectLinkService;
            _trackingService = trackingService;
        }

        [HttpGet("/redirect/{base64Properties}/{hmac}")]
        public async void Redirect(string base64Properties, string hmac, CancellationToken cancellationToken)
        {
            var redirectParams = new RedirectParams(hmac, base64Properties);
            var redirectLink = new RedirectLink(redirectParams);
            var redirectUrl = await _redirectLinkService.Redirect(redirectLink, redirectParams, cancellationToken);
            Response.Redirect(redirectUrl);
        }
        
        [HttpPost("/track")]
        public void Track([FromBody] Payload payload)
        {
            _trackingService.Track(payload);
        }

        [HttpGet("/track/{base64Payload}")]
        public void TrackGet(string base64Payload)
        {
            var data = Convert.FromBase64String(base64Payload);
            var rawJson = Encoding.UTF8.GetString(data);
            var payload = JsonConvert.DeserializeObject(rawJson, typeof(Payload)) as Payload;
            _trackingService.Track(payload);
        }
    }
}