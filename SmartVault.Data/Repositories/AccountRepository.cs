using Dapper;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using System.Data.SQLite;
using System.Linq;

namespace SmartVault.Data.Repositories
{
    public class AccountRepository : SeedInsertionRepository, IAccountRepository
    {
        private readonly IDatabaseManager _databaseManager;

        public AccountRepository(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public override SQLiteCommand CreateInsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand accountInsertCommand = connection.CreateCommand();
            accountInsertCommand.CommandText =
                "INSERT INTO Account (Id, Name, CreatedOn)" +
                "VALUES(@Id, @Name, @CreatedOn)";

            accountInsertCommand.Parameters.Add(new("@Id", System.Data.DbType.Int32));
            accountInsertCommand.Parameters.Add(new("@Name", System.Data.DbType.String));
            accountInsertCommand.Parameters.Add(new("@CreatedOn", System.Data.DbType.String));

            return accountInsertCommand;
        }

        public int GetCount()
        {
            using SQLiteConnection connection = _databaseManager.CreateConnection();
            int count = connection.Query<int>("SELECT COUNT(*) FROM Account;").FirstOrDefault();
            return count;
        }
    }
}
