using System;
using System.Text;
using Tracking.Core.Interfaces;
using Tracking.Core.Models;

namespace Tracking.Infrastructure.Clients
{
    public class DummyMessaging : IMessaging
    {
        public void Publish(Message message)
        {
            Console.WriteLine($"Sending message to topic: [{message.TopicName}] " +
                              $"| data: {Encoding.UTF8.GetString(message.Data)}");
        }
    }
}