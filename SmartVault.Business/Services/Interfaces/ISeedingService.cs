namespace SmartVault.Business.Services.Interfaces
{
    public interface ISeedingService
    {
        public void CreateDatabase();

        public void SeedDatabase(int numberOfUsers, int numberOfDocuments, string documentPath);
    }
}
