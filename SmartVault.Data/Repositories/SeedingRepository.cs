using Dapper;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using SmartVault.Library;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Xml.Serialization;

namespace SmartVault.Data.Repositories
{
    public class SeedingRepository : ISeedingRepository
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IDocumentRepository _documentRepository;

        public SeedingRepository(IDatabaseManager databaseManager,
                                 IUserRepository userRepository,
                                 IAccountRepository accountRepository,
                                 IDocumentRepository documentRepository)
        {
            _databaseManager = databaseManager;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _documentRepository = documentRepository;
        }

        public void CreateDatabase()
        {
            _databaseManager.CreateDatabase();
        }

        public void SeedDatabase(int numberOfUsers, int numberOfDocuments, string documentPath)
        {
            using SQLiteConnection connection = _databaseManager.CreateConnection();
            connection.Open();

            CreateTables(connection);
            ImproveOperationPerformance(connection);

            using SQLiteCommand userInsertCommand = _userRepository.CreateInsertCommand(connection);
            using SQLiteCommand accountInsertCommand = _accountRepository.CreateInsertCommand(connection);
            using SQLiteCommand documentInsertCommand = _documentRepository.CreateInsertCommand(connection);

            int documentNumber = 0;
            FileInfo documentInfo = new(documentPath);
            for (int index = 0; index < numberOfUsers; index++)
            {
                using SQLiteTransaction transaction = connection.BeginTransaction();
                userInsertCommand.Transaction = transaction;
                accountInsertCommand.Transaction = transaction;
                documentInsertCommand.Transaction = transaction;

                IDictionary<string, object> userParameters = CreateUserParameters(index);
                _userRepository.Insert(userInsertCommand, userParameters);

                IDictionary<string, object> accountParameters = CreateAccountParameters(index);
                _accountRepository.Insert(accountInsertCommand, accountParameters);

                for (int documentIndex = 0; documentIndex < numberOfDocuments; documentIndex++, documentNumber++)
                {
                    IDictionary<string, object> documentParameters = CreateDocumentParameters(index, documentNumber, documentIndex, documentInfo);
                    _documentRepository.Insert(documentInsertCommand, documentParameters);
                }

                transaction.Commit();
            }
        }

        private static void CreateTables(SQLiteConnection connection)
        {
            string[] files = Directory.GetFiles(@"..\..\..\..\BusinessObjectSchema");
            foreach (string file in files)
            {
                XmlSerializer serializer = new(typeof(BusinessObject));
                BusinessObject businessObject = serializer.Deserialize(new StreamReader(file)) as BusinessObject;
                connection.Execute(businessObject?.Script);
            }
        }

        private static void ImproveOperationPerformance(SQLiteConnection connection)
        {
            using SQLiteCommand pragmaCommand = connection.CreateCommand();
            pragmaCommand.CommandText = "PRAGMA synchronous = NORMAL; PRAGMA journal_mode = WAL;";
            pragmaCommand.ExecuteNonQuery();
        }

        private static IDictionary<string, object> CreateUserParameters(int index)
        {
            IEnumerator<DateTime> randomDayIterator = GenerateRandomDay().GetEnumerator();
            randomDayIterator.MoveNext();

            Dictionary<string, object> userParameters = new()
            {
                { "@Id", index },
                { "@FirstName", $"FName{index}" },
                { "@LastName", $"LName{index}" },
                { "@DateOfBirth", $"{randomDayIterator.Current:yyyy-MM-dd}" },
                { "@AccountId", index },
                { "@Username", $"UserName-{index}" },
                { "@Password", "e10adc3949ba59abbe56e057f20f883e" },
                { "@CreatedOn", $"{DateTime.Now:yyyy-MM-dd:HH:mm:ss}" }
            };

            return userParameters;
        }

        private static IDictionary<string, object> CreateAccountParameters(int index)
        {
            Dictionary<string, object> accountParameters = new()
            {
                { "@Id", index },
                { "@Name", $"Account{index}" },
                { "@CreatedOn", $"{DateTime.Now:yyyy-MM-dd:HH:mm:ss}" }
            };

            return accountParameters;
        }

        private static IDictionary<string, object> CreateDocumentParameters(int index, int documentNumber, int documentIndex, FileInfo documentInfo)
        {
            Dictionary<string, object> documentParameters = new()
            {
                { "@Id", documentNumber },
                { "@Name", $"Document{index}-{documentIndex}.txt" },
                { "@FilePath", $"{documentInfo.FullName}" },
                { "@Length", documentInfo.Length },
                { "@AccountId", index },
                { "@CreatedOn", $"{DateTime.Now:yyyy-MM-dd:HH:mm:ss}" }
            };

            return documentParameters;
        }

        private static IEnumerable<DateTime> GenerateRandomDay()
        {
            DateTime startDate = new(1985, 1, 1);
            Random randomGenerator = new();
            int dateRange = (DateTime.Today - startDate).Days;
            while (true)
            {
                yield return startDate.AddDays(randomGenerator.Next(dateRange));
            }
        }
    }
}
