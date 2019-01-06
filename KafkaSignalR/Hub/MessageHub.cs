
namespace KafkaSignalR.Hub
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using KafkaSignalR.Domain.Model;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("MessageHub")]
    public class MessageHub : Hub
    {
        public override Task OnConnected()
        {
            Clients.Caller.OnConnected(this.Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.Others.OnDisconnected(this.Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        [HttpPost]
        public void BroadCastMessage(PubMessage message)
        {
            Clients.Others.BroadCastMessage(message);
        }
    }
}