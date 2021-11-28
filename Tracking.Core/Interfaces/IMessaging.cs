using Tracking.Core.Models;

namespace Tracking.Core.Interfaces
{
    public interface IMessaging
    {
        void Publish(Message message);
    }
}