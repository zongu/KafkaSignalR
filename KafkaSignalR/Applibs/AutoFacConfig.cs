﻿
namespace KafkaSignalR.Applibs
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using KafkaSignalR.Domain.Model.Kafka;
    using KafkaSignalR.Domain.Persistent;
    using KafkaSignalR.Repository;

    internal static class AutoFacConfig
    {
        public static IContainer Container;

        public static void ContainerGenerate()
        {
            var builder = new ContainerBuilder();

            var asm = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(asm)
                .Where(t => t.IsAssignableTo<IkafkaPubSubHandler>())
                .Named<IPubSubHandler<KafkaEventStream>>(t => t.Name.Replace("Handler", string.Empty))
                .SingleInstance();

            builder.RegisterType<MsSqlPubMessageRepository>()
                    .WithParameter("connectionString", ConfigHelper.KafkaSignalRConnectionString)
                    .As<IPubMessageRepository>();

            Container = builder.Build();
        }
    }
}