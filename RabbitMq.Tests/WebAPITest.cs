using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.WebApi;

namespace RabbitMqAPI.Tests
{
    [TestClass]
    public class WebAPITest
    {
        private const string RABBIT_URL = "rabbitmq://localhost:5672/apitest_webapi";
        private const string WEBAPI_BASE_ADDRESS = "http://localhost:10026/";
        private const string MESSAGE_ROUTE = "/Message/{0}";
        private WebApiRunner _webServer;
        private IServiceBus _serviceBus;

        [TestInitialize]
        public void Initialize()
        {
            _serviceBus = Helper.SetupBusMonitoring(RABBIT_URL);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _webServer.Stop();
            _serviceBus.Dispose();
        }

        [TestMethod]
        public void SendAMessageToRabbitMQUsingWebAPI()
        {
            var messageContent = "Hola mundial";

            GivenWebApiIsRunning();

            Helper.WhenTheApiIsCalled(messageContent, MESSAGE_ROUTE, WEBAPI_BASE_ADDRESS);

            Helper.ThenTheMessageShouldHaveBeenSentToTheBus(messageContent);
        }

        private void GivenWebApiIsRunning()
        {
            // Unlike service bus, I moved this out of Initialize to help clarify the essential test conditions - 
            // that is, to clarify that the API is running. The fact that we're monitoring the bus is a little
            // more incidental and showing that directly in the test (as opposed ot the Initialize() method)
            // does aid in understanding what is being tested.

            _webServer = new WebApiRunner();

            _webServer.Start();
        }
    }
}
