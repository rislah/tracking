using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Tracking.Core.Models;
using Xunit;

namespace Tracking.Core.Tests
{
    public class RedirectLinkTest
    {
        [Theory]
        [InlineData(
            "123123",
            "eyJldmVudF9uYW1lIjoidGVzdC1ldmVudCIsImlwIjoiMTI3LjAuMC4xIiwidGFyZ2V0X3VybCI6Imh0dHBzOi8vZ29vZ2xlLmVlIn0=",
            "a244a350d8a8954b649acf3be2557842209e476c",
            true
        )]
        [InlineData(
            "123",
            "eyJldmVudF9uYW1lIjoidGVzdC1ldmVudCIsImlwIjoiMTI3LjAuMC4xIiwidGFyZ2V0X3VybCI6Imh0dHBzOi8vZ29vZ2xlLmVlIn0=",
            "a244a350d8a8954b649acf3be2557842209e476c",
            false
        )]
        [InlineData(
            "123",
            "asdf=",
            "a244a350d8a8954b649acf3be2557842209e476c",
            false
        )]
        [InlineData(
            "123123",
            "eyJldmVudF9uYW1lIjoidGVzdC1ldmVudCIsImlwIjoiMTI3LjAuMC4xIiwidGFyZ2V0X3VybCI6Imh0dHBzOi8vZ29vZ2xlLmVlIn0=",
            "a244a350d8a8954b649acf3be2557842209e47yy",
            false
        )]
        public async void TheoryRedirectLink(string secret, string base64, string expectedHmac, bool expectedResult)
        {
            var hmacSha = new HMACSHA1(Encoding.ASCII.GetBytes(secret));
            var link = new RedirectLink(new RedirectParams(expectedHmac, base64));
            var result = await link.VerifyHmac(hmacSha, expectedHmac, CancellationToken.None);
            Assert.Equal(expectedResult, result);
        }
    }
}