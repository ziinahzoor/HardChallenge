using System.Collections.Generic;

namespace SmartVault.Business.Services.Interfaces
{
    public interface IDocumentService : IBusinessObjectService
    {
        public void CreateDocument(string fullPath);

        public long GetAllDocumentSizes();

        public IEnumerable<string> GetAccountAllThirdDocumentsPaths(string accountId);

        public void CreateAccountDocument(string accountId);
    }
}
