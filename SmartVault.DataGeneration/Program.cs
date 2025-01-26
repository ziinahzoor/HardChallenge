using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data.SQLite;
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

        internal static void Main(string[] args)
        {
            if (args.Any() && args[0] == "test")
            {
                _isTest = true;
                _numberOfUsers = 1;
                _numberOfDocuments = 2;
            }

            GenerateDatabase();
        }

        static void GenerateDatabase()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string databaseFileName = configuration["DatabaseFileName"] ?? "";

            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            SQLiteConnection.CreateFile(databaseFileName);

            string documentPath = Path.Combine(Directory.GetCurrentDirectory(), "TestDoc.txt");
            File.WriteAllText(documentPath, GenerateTestText());

            string connectionString = string.Format(configuration["ConnectionStrings:DefaultConnection"] ?? "", databaseFileName);
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            GenerateTables(connection);
            PopulateDatabase(connection, documentPath);

            if (_isTest)
            {
                OutputInsertionStatistics(connection);
            }
        }

        static string GenerateTestText()
        {
            const int numberOfLines = 100;
            return string.Join(Environment.NewLine, new string[numberOfLines].Select(_ => "This is my test document"));
        }

        static void OutputInsertionStatistics(SQLiteConnection connection)
        {
            var accountData = connection.Query("SELECT COUNT(*) FROM Account;");
            Console.WriteLine($"AccountCount: {JsonConvert.SerializeObject(accountData)}");
            var documentData = connection.Query("SELECT COUNT(*) FROM Document;");
            Console.WriteLine($"DocumentCount: {JsonConvert.SerializeObject(documentData)}");
            var userData = connection.Query("SELECT COUNT(*) FROM User;");
            Console.WriteLine($"UserCount: {JsonConvert.SerializeObject(userData)}");
        }
    }
}