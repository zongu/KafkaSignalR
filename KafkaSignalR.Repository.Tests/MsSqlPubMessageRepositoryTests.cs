
namespace KafkaSignalR.Repository.Tests
{
    using System.Data.SqlClient;
    using Dapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MsSqlPubMessageRepositoryTests
    {
        [TestInitialize]
        public void Init()
        {
            using (var cn = new SqlConnection(Applibs.ConfigHelper.KafkaSignalRConnectionString))
            {
                cn.Execute("TRUNCATE TABLE PubMessage");
            }
        }

        [TestMethod]
        public void TestAdd()
        {
            var repo = new MsSqlPubMessageRepository(Applibs.ConfigHelper.KafkaSignalRConnectionString);
            var result = repo.Add(new Domain.Model.PubMessage()
            {
                Content = "Test"
            });

            Assert.IsNull(result.Item1);
            Assert.IsNotNull(result.Item2);
        }
    }
}
