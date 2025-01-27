using System.Collections.Generic;

namespace SmartVault.Data.Repositories.Interfaces
{
    public interface IDocumentRepository : ISeedInsertionRepository, IBusinessObjectRepository
    {
        public IEnumerable<string> GetAllDocumentsPaths();
    }
}
