using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System;
using System.Data.SQLite;
using System.IO;
using Xunit;

namespace SmartVault.Tests
{
    public class DataGenerationTests
    {
        [Fact]
        public void ItShouldCreateTestDatabase()
        {
            //Arrange
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string databaseFileName = configuration["DatabaseFileName"] ?? "";
            string connectionString = string.Format(configuration["ConnectionStrings:DefaultConnection"] ?? "", databaseFileName);
            using var connection = new SQLiteConnection(connectionString);

            //Act
            string[] args = { "test" };
            DataGeneration.Program.Main(args);
            var userCount = connection.QueryFirst<int>("SELECT COUNT(*) FROM User;");
            var accountCount = connection.QueryFirst<int>("SELECT COUNT(*) FROM Account;");
            var documentCount = connection.QueryFirst<int>("SELECT COUNT(*) FROM Document;");

            //Assert
            Assert.Equal(1, userCount);
            Assert.Equal(1, accountCount);
            Assert.Equal(2, documentCount);
        }
    }
}
