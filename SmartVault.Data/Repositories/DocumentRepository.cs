using Dapper;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using System.Data.SQLite;
using System.Linq;

namespace SmartVault.Data.Repositories
{
    public class DocumentRepository : SeedInsertionRepository, IDocumentRepository
    {
        private readonly IDatabaseManager _databaseManager;

        public DocumentRepository(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public override SQLiteCommand CreateInsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand documentInsertCommand = connection.CreateCommand();
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

        public int GetCount()
        {
            using SQLiteConnection connection = _databaseManager.CreateConnection();
            int count = connection.Query<int>("SELECT COUNT(*) FROM Document;").FirstOrDefault();
            return count;
        }
    }
}
