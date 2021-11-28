using System.Threading;
using System.Threading.Tasks;
using Tracking.Core.Interfaces;
using Tracking.Core.Models;

namespace Tracking.Core.Services
{
    public interface IRedirectLinkService
    {
        Task<string> Redirect(ILink link, RedirectParams redirectParams, CancellationToken cancellationToken);
    }
}