using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace RabbitMqAPI.Tests
{
    public static class Helper
    {
        public static void WhenTheApiIsCalled(string messageContent, string routing, string baseAddress)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(string.Format(routing, System.Net.WebUtility.UrlEncode(messageContent)));

                Assert.AreEqual(HttpStatusCode.Accepted, response.Result.StatusCode, "expected code 202");
            }

            Thread.Sleep(500);
        }


        public static IServiceBus SetupBusMonitoring(string queueAddress)
        {
            var bus = ServiceBusFactory.New(x =>
            {
                x.UseRabbitMq();
                x.ReceiveFrom(queueAddress);
                x.DisablePerformanceCounters();

                x.Subscribe(s =>
                {
                    s.Consumer(() => new ApiTestConsumer());
                });
            });

            return bus;
        }

        public static void ThenTheMessageShouldHaveBeenSentToTheBus(string messageContent)
        {
            Assert.AreEqual(messageContent, ApiTestConsumer.ReceivedMessage, "proper message was not received");
        }
    }
}
