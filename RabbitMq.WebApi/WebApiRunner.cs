using Microsoft.Owin.Hosting;
using System;

namespace RabbitMQ.WebApi
{
    public class WebApiRunner
    {
        public static void Main()
        {
            try
            {
                var program = new WebApiRunner();
                program.Start();

                Console.WriteLine("Press any key to exit.");
                Console.Read();

                program.Stop();

            }
            catch (Exception exception)
            {
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine(exception.Message);
                Console.WriteLine("--------------------------------------------------------");
            }
        }

        private IDisposable _webService;

        public void Start()
        {
            var baseAddress = Settings.WebServiceBaseAddress;
            _webService = WebApp.Start<Startup>(baseAddress);

            Console.WriteLine("WebAPI is running at " + baseAddress);
        }

        public void Stop()
        {
            _webService.Dispose();

            Console.WriteLine("WebAPI has stopped running");
        }
    }
}
