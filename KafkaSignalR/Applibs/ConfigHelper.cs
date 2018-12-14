
namespace KafkaSignalR.Applibs
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class ConfigHelper
    {
        public static readonly string KafkaBorkerList = ConfigurationManager.AppSettings["KafkaBorkerList"];

        public static readonly int KafkaProducerRequiredAcks = int.Parse(ConfigurationManager.AppSettings["KafkaProducerRequiredAcks"]);

        public static readonly int KafkaConsumerNumbers = int.Parse(ConfigurationManager.AppSettings["KafkaConsumerNumbers"]);

        public static readonly string KafkaConsumerGroupId = ConfigurationManager.AppSettings["KafkaConsumerGroupId"];

        public static readonly IEnumerable<string> KafkaConsumerTopics = ConfigurationManager.AppSettings["KafkaConsumerTopics"].Split(',').ToList();

        public static readonly string KafkaSignalRConnectionString = ConfigurationManager.ConnectionStrings["KafkaSignalRConnectionString"].ConnectionString;
    }
}