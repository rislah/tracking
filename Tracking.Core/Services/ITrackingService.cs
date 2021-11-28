
using Tracking.Core.Models;

namespace Tracking.Core.Services
{
    public interface ITrackingService
    {
        void Track(Payload payload);
    }
}