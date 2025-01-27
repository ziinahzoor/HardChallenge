namespace SmartVault.Business.Services.Interfaces
{
    public interface IDocumentService : IBusinessObjectService
    {
        public void CreateDocument(string fullPath);
    }
}
