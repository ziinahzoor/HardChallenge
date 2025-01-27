using SmartVault.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SmartVault.Data.Repositories
{
    public abstract class SeedInsertionRepository : ISeedInsertionRepository
    {
        public void Insert(SQLiteCommand command, IDictionary<string, object> parameters)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                command.Parameters[parameter.Key].Value = parameter.Value;
            }

            command.ExecuteNonQuery();
        }

        public abstract SQLiteCommand CreateInsertCommand(SQLiteConnection connection);
    }
}
