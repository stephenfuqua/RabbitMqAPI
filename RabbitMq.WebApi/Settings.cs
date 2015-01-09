using System;

namespace RabbitMQ.WebApi
{
    public static class Settings
    {

        public static Uri ServiceBusAddress
        {
            get { return new Uri("rabbitmq://localhost:5672/apitest_webapi"); }
        }


        public static string WebServiceBaseAddress {
            get { return "http://localhost:10026/"; }
        }


    }
}
