
namespace KafkaSignalR.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Logging;
    using KafkaSignalR.Domain.Model;
    using KafkaSignalR.Domain.Model.Event;

    public class PubMessageController : ApiController
    {
        private ILog log = LogManager.GetLogger<PubMessageController>();

        [HttpPost]
        public HttpResponseMessage Post(PostInput input)
        {
            try
            {
                var @event = new PubMessageEvent()
                {
                    Content = input.Message
                };

                Applibs.KafkaEventProducer.Producer.Publish(nameof(PubMessage), 0, @event);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                this.log.Error($"PubMessage Post Exception:{ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public class PostInput
        {
            public string Message { get; set; }
        }
    }
}
