namespace SmartVault.Data.Repositories.Interfaces
{
    public interface ISeedingRepository
    {
        public void CreateDatabase();

        public void SeedDatabase(int numberOfUsers, int numberOfDocuments, string documentPath);
    }
}
