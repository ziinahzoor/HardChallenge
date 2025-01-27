using SmartVault.Business.Services.Interfaces;
using SmartVault.Data.Repositories.Interfaces;

namespace SmartVault.Business.Services
{
    public class SeedingService : ISeedingService
    {
        private readonly ISeedingRepository _seedingRepository;

        public SeedingService(ISeedingRepository seedingRepository)
        {
            _seedingRepository = seedingRepository;
        }

        public void CreateDatabase()
        {
            _seedingRepository.CreateDatabase();
        }

        public void SeedDatabase(int numberOfUsers, int numberOfDocuments, string documentPath)
        {
            _seedingRepository.SeedDatabase(numberOfUsers, numberOfDocuments, documentPath);
        }
    }
}
