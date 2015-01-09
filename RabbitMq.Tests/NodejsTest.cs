using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Diagnostics;

namespace RabbitMqAPI.Tests
{
    [TestClass]
    public class NodejsTest
    {
        private IServiceBus _serviceBus;
        private const string RABBIT_URL = "rabbitmq://localhost:5672/apitest_webapi";
        private const string NODEJS_URL = "http://localhost:10025/";
        private const string MESSAGE_REQUEST = "Message/{0}";
        private const string NODEJS_PACKAGE = @"""C:\Users\sfuqua\Documents\Visual Studio 2013\Projects\RabbitMqAPI\app.js""";
        private const string NODE_EXECUTABLE = @"""C:\Program Files\nodejs\node.exe""";
        private Process _webServer;

        [TestInitialize]
        public void Initialize()
        {
            _serviceBus = Helper.SetupBusMonitoring(RABBIT_URL);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _serviceBus.Dispose();

            if (!_webServer.HasExited)
            {
                _webServer.Kill();
                _webServer.Dispose();
            }
        }

        [TestMethod]
        public void SendAMessageToRabbitMQUsingNodeJs()
        {
            var messageContent = "one small step for a man...";

            GivenNodeJsApiIsRunning();
            Helper.WhenTheApiIsCalled(messageContent, MESSAGE_REQUEST, NODEJS_URL);

            Helper.ThenTheMessageShouldHaveBeenSentToTheBus(messageContent);
        }


        private void GivenNodeJsApiIsRunning()
        {
            var processStartInfo = new ProcessStartInfo
            {
                Arguments = NODEJS_PACKAGE,
                FileName = NODE_EXECUTABLE,
                ErrorDialog = true,
                UseShellExecute = true
            };

            try
            {
                _webServer = Process.Start(processStartInfo);
            }
            catch (Win32Exception exception)
            {
                Assert.Fail(exception.ToString());
            }
        }


    }
}
