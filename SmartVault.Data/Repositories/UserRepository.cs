using Dapper;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using System.Data.SQLite;
using System.Linq;

namespace SmartVault.Data.Repositories
{
    public class UserRepository : SeedInsertionRepository, IUserRepository
    {
        private readonly IDatabaseManager _databaseManager;

        public UserRepository(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public override SQLiteCommand CreateInsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand userInsertCommand = connection.CreateCommand();
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

        public int GetCount()
        {
            using SQLiteConnection connection = _databaseManager.CreateConnection();
            int count = connection.Query<int>("SELECT COUNT(*) FROM User;").FirstOrDefault();
            return count;
        }
    }
}
