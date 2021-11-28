using System;
using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Tracking.Core.Interfaces;
using Tracking.Core.Models;

namespace Tracking.Infrastructure.Clients
{
    public class KafkaMessaging : IMessaging
    {
        private readonly IProducer<Null, byte[]> _producer;

        public KafkaMessaging(string bootstrapServers)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
            };

            _producer = new ProducerBuilder<Null, byte[]>(config).Build();
        }

        public async void Publish(Message message)
        {
            await _producer.ProduceAsync(message.TopicName, new Message<Null, byte[]>() {Value = message.Data});
        }

        ~KafkaMessaging()
        {
            _producer.Dispose();
        }
    }
}