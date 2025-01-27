using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartVault.Business.Services;
using SmartVault.Business.Services.Interfaces;
using SmartVault.Data;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories;
using SmartVault.Data.Repositories.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SmartVault.Tests")]
namespace SmartVault.DataGeneration
{
    internal partial class Program
    {
        static bool _isTest = false;
        static int _numberOfUsers = 100;
        static int _numberOfDocuments = 10000;

        static readonly ISeedingService _seedingService;
        static readonly IUserService _userService;
        static readonly IAccountService _accountService;
        static readonly IDocumentService _documentService;

        static Program()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            ServiceProvider serviceProvider = new ServiceCollection()
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

            _seedingService = serviceProvider.GetRequiredService<ISeedingService>();
            _userService = serviceProvider.GetRequiredService<IUserService>();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
            _documentService = serviceProvider.GetRequiredService<IDocumentService>();
        }

        internal static void Main(string[] args)
        {
            if (args.Any() && args[0] == "test")
            {
                _isTest = true;
                _numberOfUsers = 1;
                _numberOfDocuments = 2;
            }

            SeedDatabase();

            if (!_isTest)
            {
                OutputInsertionStatistics();
            }
        }

        static void SeedDatabase()
        {
            string documentPath = Path.Combine(Directory.GetCurrentDirectory(), "TestDoc.txt");
            _documentService.CreateDocument(documentPath);

            _seedingService.CreateDatabase();
            _seedingService.SeedDatabase(_numberOfUsers, _numberOfDocuments, documentPath);
        }

        static void OutputInsertionStatistics()
        {
            int accountCount = _accountService.GetCount();
            int documentCount = _documentService.GetCount();
            int userCount = _userService.GetCount();
            Console.WriteLine($"AccountCount: { accountCount }");
            Console.WriteLine($"DocumentCount: { documentCount }");
            Console.WriteLine($"UserCount: { userCount }");
        }
    }
}