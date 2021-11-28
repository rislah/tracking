using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Tracking.Core.Interfaces
{
    public interface ILink
    {
        Task<bool> VerifyHmac(HMACSHA1 hmacSha, string expected, CancellationToken cancellationToken);
    }
}