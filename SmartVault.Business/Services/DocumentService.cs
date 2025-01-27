using SmartVault.Business.Services.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace SmartVault.Business.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public int GetCount() => _documentRepository.GetCount();

        public void CreateDocument(string fullPath)
        {
            File.WriteAllText(fullPath, GenerateTestText());
        }

        static string GenerateTestText()
        {
            const int numberOfLines = 100;
            return string.Join(Environment.NewLine, new string[numberOfLines].Select(_ => "This is my test document"));
        }
    }
}
