using Dapper;
using SmartVault.Data.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using System.Data.SQLite;
using System.Linq;

namespace SmartVault.Data.Repositories
{
    public class OAuthUserRepository : SeedInsertionRepository, IOAuthUserRepository
    {
        private readonly IDatabaseManager _databaseManager;

        public OAuthUserRepository(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public override SQLiteCommand CreateInsertCommand(SQLiteConnection connection)
        {
            SQLiteCommand userInsertCommand = connection.CreateCommand();
            userInsertCommand.CommandText =
                "INSERT INTO OAuthUser (Id, LocalId, EmailAddress, AccessToken, RefreshToken, TokenExpiration, Provider, CreatedOn)" +
                "VALUES(@Id, @LocalId, @EmailAddress, @AccessToken, @RefreshToken, @TokenExpiration, @Provider, @CreatedOn)";

            userInsertCommand.Parameters.Add(new("@Id", System.Data.DbType.Int32));
            userInsertCommand.Parameters.Add(new("@LocalId", System.Data.DbType.Int32));
            userInsertCommand.Parameters.Add(new("@EmailAddress", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@AccessToken", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@RefreshToken", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@TokenExpiration", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@Provider", System.Data.DbType.String));
            userInsertCommand.Parameters.Add(new("@CreatedOn", System.Data.DbType.String));

            return userInsertCommand;
        }

        public int GetCount()
        {
            using SQLiteConnection connection = _databaseManager.CreateConnection();
            int count = connection.Query<int>("SELECT COUNT(*) FROM OAuthUser;").FirstOrDefault();
            return count;
        }
    }
}
