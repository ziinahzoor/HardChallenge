using System.Data.SQLite;

namespace SmartVault.Data.Interfaces
{
    public interface IDatabaseManager
    {
        public void CreateDatabase();

        public SQLiteConnection CreateConnection();
    }
}
