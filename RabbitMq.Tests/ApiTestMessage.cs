using MassTransit;
using System;

namespace RabbitMqAPI.Tests
{
    public class ApiTestMessage : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public string MessageContent { get; set; }
    }
}
