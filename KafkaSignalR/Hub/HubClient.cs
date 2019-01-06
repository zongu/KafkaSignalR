
namespace KafkaSignalR.Hub
{
    using System;
    using KafkaSignalR.Applibs;
    using KafkaSignalR.Domain.Model;
    using Microsoft.AspNet.SignalR.Client;

    public class HubClient
    {
        private static HubConnection connection;

        private static IHubProxy myHub;

        private static void UseConnection(Action<IHubProxy> action)
        {
            if (myHub == null)
            {
                connection = new HubConnection($"{ConfigHelper.SignalRUrl}/signalr/hubs");
                myHub = connection.CreateHubProxy("MessageHub");
            }

            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    return;
                }
                else
                {
                    action(myHub);
                    connection.Stop();
                }
            }).Wait();
        }

        public static void BocastMessage(PubMessage message)
        {
            UseConnection(hubPrx =>
            {
                hubPrx.Invoke<PubMessage>("BroadCastMessage", message)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                    }
                }).Wait();
            });
        }
    }
}