using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartVault.Business.Services.Interfaces;
using SmartVault.Business.Services;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using SmartVault.Data.Repositories;
using SmartVault.Data;
using System.IO;
using Xunit;
using System.Collections.Generic;

namespace SmartVault.Tests
{
    [Collection("DatabaseAccessRequirement")]
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
            long totalFileSize = documentService.GetAllDocumentSizes();

            //Assert
            Assert.Equal(fileSize * 6, totalFileSize);
        }

        [Fact]
        public void ItShouldGenerateADocumentWithAllThirdDocuments()
        {
            //Arrange
            string[] args = { "test" };
            DataGeneration.Program.Main(args);
            IDocumentService documentService = _serviceProvider.GetService<IDocumentService>();
            IEnumerable<string> allThirdDocuments = documentService.GetAccountAllThirdDocumentsPaths("0");
            int totalCharacters = 0;
            int newFileCharacterSpacing = 4;

            foreach(var document in allThirdDocuments)
            {
                string documentContent = File.ReadAllText(document);
                if (documentContent.Contains("Smith Property"))
                {
                    totalCharacters += documentContent.Length + newFileCharacterSpacing;
                }
            }

            //Act
            documentService.CreateAccountDocument("0");
            string finalDocument = File.ReadAllText("Account0Document.txt");

            //Assert
            Assert.Equal(finalDocument.Length, totalCharacters);
        }
    }
}
