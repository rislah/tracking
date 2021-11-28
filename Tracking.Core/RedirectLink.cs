using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracking.Core.Interfaces;
using Tracking.Core.Models;

namespace Tracking.Core
{
    public class RedirectLink : ILink
    {
        private readonly string _propertiesBase64;

        public RedirectLink(RedirectParams redirectParams)
        {
            _propertiesBase64 = redirectParams.PropertiesBase64;
        }

        public async Task<bool> VerifyHmac(HMACSHA1 hmacSha, string expectedHash, CancellationToken cancellationToken)
        {
            await using var stream = new MemoryStream(Encoding.ASCII.GetBytes(_propertiesBase64));
            var result = await hmacSha.ComputeHashAsync(stream, cancellationToken);
            var hash = result.Aggregate("", (s, b) => s + $"{b:x2}", s => s);
            return hash == expectedHash;
        }
    }
}