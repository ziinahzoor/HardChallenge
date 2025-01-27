using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartVault.Business.Services.Interfaces;
using SmartVault.Business.Services;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using SmartVault.Data;
using SmartVault.Data.Repositories;
using System;

namespace SmartVault.Program
{
    partial class Program
    {
        static readonly IDocumentService _documentService;

        static Program()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IDocumentService, DocumentService>()
                .AddSingleton<IDatabaseManager, DatabaseManager>()
                .AddSingleton<IDocumentRepository, DocumentRepository>()
                .BuildServiceProvider();

            _documentService = serviceProvider.GetRequiredService<IDocumentService>();
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            WriteEveryThirdFileToFile(args[0]);
            GetAllFileSizes();
        }

        private static void GetAllFileSizes()
        {
            long totalSize = _documentService.GetAllFileSizes();
            Console.WriteLine($"Total file size: {totalSize}b");
        }

        private static void WriteEveryThirdFileToFile(string accountId)
        {
            // TODO: Implement functionality
        }
    }
}