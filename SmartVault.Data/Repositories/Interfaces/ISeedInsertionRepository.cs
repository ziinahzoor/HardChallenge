using System.Collections.Generic;
using System.Data.SQLite;

namespace SmartVault.Data.Repositories.Interfaces
{
    public interface ISeedInsertionRepository
    {
        public void Insert(SQLiteCommand command, IDictionary<string, object> parameters);

        public SQLiteCommand CreateInsertCommand(SQLiteConnection connection);
    }
}
