using MassTransit;
using RabbitMQ.WebApi;
using System.Net;

namespace RabbitMqAPI.Tests
{
    public class ApiTestConsumer : Consumes<SimpleMessage>.Context
    {
        public static string ReceivedMessage { get; private set; }

        public ApiTestConsumer()
        {
            ReceivedMessage = string.Empty;
        }

        public void Consume(IConsumeContext<SimpleMessage> busContext)
        {
            ReceivedMessage =  WebUtility.UrlDecode(busContext.Message.Content);
        }
    }
}
