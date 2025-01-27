using Microsoft.Extensions.Configuration;
using SmartVault.Data.Interfaces;
using System.Data.SQLite;
using System.IO;
namespace SmartVault.Data
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly IConfiguration _configuration;

        public DatabaseManager(IConfiguration configuration) => _configuration = configuration;

        public void CreateDatabase()
        {
            string databaseFileName = _configuration["DatabaseFileName"] ?? "";

            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            SQLiteConnection.CreateFile(databaseFileName);
        }

        public SQLiteConnection CreateConnection()
        {
            string connectionString = string.Format(_configuration["ConnectionStrings:DefaultConnection"] ?? "", _configuration["DatabaseFileName"] ?? "");
            return new SQLiteConnection(connectionString);
        }
    }
}
