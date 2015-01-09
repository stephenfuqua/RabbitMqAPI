using MassTransit;
using Microsoft.Practices.Unity;
using Owin;
using System;
using System.Net.Http.Headers;
using System.Web.Http;

namespace RabbitMQ.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException("appBuilder");
            }

            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config = SetupContentHeaders(config);
            config = SetupDependencyInjection(config);


            appBuilder.UseWebApi(config);
        }


        private static HttpConfiguration SetupDependencyInjection(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterInstance<IServiceBus>(InitializeServiceBus(), new ContainerControlledLifetimeManager());


            config.DependencyResolver = new UnityResolver(container);
            return config;
        }

        private static HttpConfiguration SetupContentHeaders(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            return config;
        }

        private static IServiceBus InitializeServiceBus()
        {

            var serviceBus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseRabbitMq();
                sbc.ReceiveFrom(Settings.ServiceBusAddress);
                sbc.DisablePerformanceCounters();
            });

            return serviceBus;
        }
    }
}
