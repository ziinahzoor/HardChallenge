using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartVault.Business.Services.Interfaces;
using SmartVault.Business.Services;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using SmartVault.Data.Repositories;
using SmartVault.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SmartVault.Tests
{
    public class ProgramTests
    {
        private readonly ServiceProvider _serviceProvider;

        public ProgramTests()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IDocumentService, DocumentService>()
                .AddSingleton<IDatabaseManager, DatabaseManager>()
                .AddSingleton<IDocumentRepository, DocumentRepository>()
                .BuildServiceProvider();
        }

        [Fact]
        public void ItShouldGetAllFilesSize()
        {
            //Arrange
            string[] args = { "test" };
            DataGeneration.Program.Main(args);
            IDocumentService documentService = _serviceProvider.GetService<IDocumentService>();
            documentService.CreateDocument("TestDoc.txt");
            long fileSize = new FileInfo("TestDoc.txt").Length;

            //Act
            long totalFileSize = documentService.GetAllFileSizes();

            //Assert
            Assert.Equal(fileSize * 2, totalFileSize);
        }
    }
}
