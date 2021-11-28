using System.Collections.Generic;

namespace Tracking.Core
{
    public static class EventTopicStore
    {
        private static readonly Dictionary<string, string> EventTopicDict = new()
        {
            {"user_login_attempt", Constants.UserLoginAttemptTopic},
            {"email_open", Constants.EmailOpenTopic}
        };

        public static string GetTopic(string eventName)
        {
            return EventTopicDict[eventName];
        }
    }
}