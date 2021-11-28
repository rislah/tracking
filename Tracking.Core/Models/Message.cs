namespace Tracking.Core.Models
{
    public class Message
    {
        public readonly byte[] Data;

        public readonly string TopicName;

        public Message(string topicName, byte[] data)
        {
            TopicName = topicName;
            Data = data;
        }
    }
}