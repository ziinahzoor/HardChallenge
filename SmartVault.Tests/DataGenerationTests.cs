using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartVault.Business.Services.Interfaces;
using SmartVault.Business.Services;
using SmartVault.Data.Repositories.Interfaces;
using SmartVault.Data.Repositories;
using System.IO;
using Xunit;
using SmartVault.Data.Interfaces;
using SmartVault.Data;

namespace SmartVault.Tests
{
    public class DataGenerationTests
    {
        private readonly ServiceProvider _serviceProvider;

        public DataGenerationTests()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<ISeedingService, SeedingService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IAccountService, AccountService>()
                .AddSingleton<IDocumentService, DocumentService>()
                .AddSingleton<IDatabaseManager, DatabaseManager>()
                .AddSingleton<ISeedingRepository, SeedingRepository>()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IAccountRepository, AccountRepository>()
                .AddSingleton<IDocumentRepository, DocumentRepository>()
                .BuildServiceProvider();
        }

        [Fact]
        public void ItShouldCreateTestDatabase()
        {
            //Arrange
            string[] args = { "test" };
            IUserService userService = _serviceProvider.GetService<IUserService>();
            IAccountService accountService = _serviceProvider.GetService<IAccountService>();
            IDocumentService documentService = _serviceProvider.GetService<IDocumentService>();

            //Act
            DataGeneration.Program.Main(args);
            int userCount = userService.GetCount();
            int accountCount = accountService.GetCount();
            int documentCount = documentService.GetCount();

            //Assert
            Assert.Equal(1, userCount);
            Assert.Equal(1, accountCount);
            Assert.Equal(2, documentCount);
        }
    }
}
