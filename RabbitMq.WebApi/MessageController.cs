using MassTransit;
using RabbitMQ.WebApi;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Trintech.Cadency.Completion.API
{
    public class MessageController : ApiController
    {
        private readonly IServiceBus _serviceBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        /// <param name="serviceBus">The service bus.</param>
        public MessageController(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        [Route("Message/{message}")]
        public HttpResponseMessage Get(string message)
        {
            SendToQueue(message);

            return new HttpResponseMessage(HttpStatusCode.Accepted);
            //var response = CreateResponse(message);
            // return response;
        }

        private void SendToQueue(string message)
        {
            _serviceBus.Publish(new SimpleMessage { Content = message });
        }

        //private HttpResponseMessage CreateResponse(string message)
        //{
        //    var response = Request.CreateResponse(HttpStatusCode.Accepted);

        //    response.Content = new StringContent("<html><body><p>Recieved: " + message + "</p></body></html>");
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        //    return response;
        //}
    }




}
