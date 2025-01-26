using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace SmartVault.DataGeneration
{
    internal partial class Program
    {
        static void PopulateDatabase(SQLiteConnection connection, string documentPath)
        {
            int documentNumber = 0;
            var documentInfo = new FileInfo(documentPath);

            using var pragmaCommand = connection.CreateCommand();
            pragmaCommand.CommandText = "PRAGMA synchronous = NORMAL; PRAGMA journal_mode = WAL;";
            pragmaCommand.ExecuteNonQuery();

            using var userInsertCommand = CreateUserInsertCommand(connection);
            using var accountInsertCommand = CreateAccountInsertCommand(connection);
            using var documentInsertCommand = CreateDocumentInsertCommand(connection);

            for (int i = 0; i < _numberOfUsers; i++)
            {
                using var transaction = connection.BeginTransaction();
                userInsertCommand.Transaction = transaction;
                accountInsertCommand.Transaction = transaction;
                documentInsertCommand.Transaction = transaction;

                var randomDayIterator = GenerateRandomDay().GetEnumerator();
                randomDayIterator.MoveNext();

                var userParameters = new Dictionary<string, object>() { { "@Id", i }, { "@FirstName", $"FName{i}" }, { "@LastName", $"LName{i}" }, { "@DateOfBirth", $"{randomDayIterator.Current:yyyy-MM-dd}" }, { "@AccountId", i }, { "@Username", $"UserName-{i}" }, { "@Password", "e10adc3949ba59abbe56e057f20f883e" }, { "@CreatedOn", $"{DateTime.Now:yyyy-MM-dd:HH:mm:ss}" } };
                SetParameters(userInsertCommand, userParameters);
                userInsertCommand.ExecuteNonQuery();

                var accountParameters = new Dictionary<string, object>() { { "@Id", i }, { "@Name", $"Account{i}" }, { "@CreatedOn", $"{DateTime.Now:yyyy-MM-dd:HH:mm:ss}" } };
                SetParameters(accountInsertCommand, accountParameters);
                accountInsertCommand.ExecuteNonQuery();

                for (int d = 0; d < _numberOfDocuments; d++, documentNumber++)
                {
                    var documentParameters = new Dictionary<string, object>() { { "@Id", documentNumber }, { "@Name", $"Document{i}-{d}.txt" }, { "@FilePath", $"{documentInfo.FullName}" }, { "@Length", documentInfo.Length }, { "@AccountId", i }, { "@CreatedOn", $"{DateTime.Now:yyyy-MM-dd:HH:mm:ss}" } };
                    SetParameters(documentInsertCommand, documentParameters);
                    documentInsertCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }

        static SQLiteCommand CreateUserInsertCommand(SQLiteConnection connection)
        {
            var userInsertCommand = connection.CreateCommand();
            userInsertCommand.CommandText =
                "INSERT INTO User (Id, FirstName, LastName, DateOfBirth, AccountId, Username, Password, CreatedOn)" +
                "VALUES(@Id, @FirstName, @LastName, @DateOfBirth, @AccountId, @Username, @Password, @CreatedOn)";

            userInsertCommand.Parameters.Add(new("@Id", System.Data.DbType.Int32));
            userInsertCommand.Parameters.Add(new("@FirstName", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@LastName", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@DateOfBirth", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@AccountId", System.Data.DbType.Int32));
            userInsertCommand.Parameters.Add(new("@Username", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@Password", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@CreatedOn", System.Data.DbType.String));

            return userInsertCommand;
        }

        static SQLiteCommand CreateAccountInsertCommand(SQLiteConnection connection)
        {
            var accountInsertCommand = connection.CreateCommand();
            accountInsertCommand.CommandText =
                "INSERT INTO Account (Id, Name, CreatedOn)" +
                "VALUES(@Id, @Name, @CreatedOn)";

            accountInsertCommand.Parameters.Add(new("@Id", System.Data.DbType.Int32));
            accountInsertCommand.Parameters.Add(new("@Name", System.Data.DbType.String));
            accountInsertCommand.Parameters.Add(new("@CreatedOn", System.Data.DbType.String));

            return accountInsertCommand;
        }

        static SQLiteCommand CreateDocumentInsertCommand(SQLiteConnection connection)
        {
            var documentInsertCommand = connection.CreateCommand();
            documentInsertCommand.CommandText =
                "INSERT INTO Document (Id, Name, FilePath, Length, AccountId, CreatedOn)" +
                "VALUES(@Id, @Name, @FilePath, @Length, @AccountId, @CreatedOn)";

            documentInsertCommand.Parameters.Add(new("@Id", System.Data.DbType.Int32));
            documentInsertCommand.Parameters.Add(new("@Name", System.Data.DbType.String));
            documentInsertCommand.Parameters.Add(new("@FilePath", System.Data.DbType.String));
            documentInsertCommand.Parameters.Add(new("@Length", System.Data.DbType.Int32));
            documentInsertCommand.Parameters.Add(new("@AccountId", System.Data.DbType.Int32));
            documentInsertCommand.Parameters.Add(new("@CreatedOn", System.Data.DbType.String));

            return documentInsertCommand;
        }

        static void SetParameters(SQLiteCommand command, Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters[parameter.Key].Value = parameter.Value;
            }
        }

        static IEnumerable<DateTime> GenerateRandomDay()
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
