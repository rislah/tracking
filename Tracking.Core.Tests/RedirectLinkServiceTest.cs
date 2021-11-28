using System;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Tracking.Core.Interfaces;
using Tracking.Core.Models;
using Tracking.Core.Services;
using Xunit;
using Xunit.Abstractions;

namespace Tracking.Core.Tests
{
    public class RedirectLinkServiceTest
    {
        [Theory]
        [InlineData(
            "c8dc6b465ccbda34ec173e956590307d514670d5",
            "eyJldmVudF9uYW1lIjoidHJhY2tpbmcuY2xpY2sudXNlci5lbWFpbG9wZW4iLCJpcCI6IjEyNy4wLjAuMSIsInRhcmdldF91cmwiOiJodHRwczovL2dvb2dsZS5lZSIsInVzZXJfaWQiOiJ1c2VyaWQtMTIzNDM5NDUiLCJlbWFpbF9pZCI6ImVtYWlsaWQtMzE1OTEzNTEifQ==",
            "https://google.ee",
            true
        )]
        [InlineData(
            "c8dc6b465ccbda34ec173e956590307d514670d9",
            "eyJldmVudF9uYW1lIjoidHJhY2tpbmcuY2xpY2sudXNlci5lbWFpbG9wZW4iLCJpcCI6IjEyNy4wLjAuMSIsInRhcmdldF91cmwiOiJodHRwczovL2dvb2dsZS5lZSIsInVzZXJfaWQiOiJ1c2VyaWQtMTIzNDM5NDUiLCJlbWFpbF9pZCI6ImVtYWlsaWQtMzE1OTEzNTEifQ==",
            Constants.FallbackRedirectUrl,
            false
        )]
        public async void TheoryRedirectLinkService(
            string hmac,
            string base64Properties,
            string expectedRedirectUrl,
            bool publishedMessage
        )
        {
            Message? message = null;
            var mockMessaging = new Mock<IMessaging>();
            mockMessaging.Setup(m => m.Publish(It.IsAny<Message>())).Callback<Message>((m) =>
            {
                message = m;
            });

            var mockLink = new Mock<ILink>();
            mockLink.Setup(m => m.VerifyHmac(It.IsAny<HMACSHA1>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(publishedMessage);
            
            var redirectParams = new RedirectParams(
                hmac,
                base64Properties
            );
            var svc = new RedirectLinkService(mockMessaging.Object, new Logger<RedirectLinkService>(new NullLoggerFactory()));
            var url = await svc.Redirect(mockLink.Object, redirectParams, CancellationToken.None);

            Assert.Equal(expectedRedirectUrl, url);
            
            if (publishedMessage)
                Assert.NotNull(message);
        }
    }
}