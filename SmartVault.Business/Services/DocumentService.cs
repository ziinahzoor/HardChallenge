using SmartVault.Business.Services.Interfaces;
using SmartVault.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

        public long GetAllDocumentSizes()
        {
            IEnumerable<string> documentPaths = _documentRepository.GetAllDocumentsPaths();
            IEnumerable<long> fileLengths = documentPaths.Select(d => new FileInfo(d).Length);
            long totalLength = fileLengths.Sum();
            return totalLength;
        }

        public IEnumerable<string> GetAccountAllThirdDocumentsPaths(string accountId)
        {
            IEnumerable<string> documentPaths = _documentRepository.GetAllDocumentsPaths(accountId);
            IEnumerable<string> thirdDocuments = documentPaths.Where((_, index) => (index + 1) % 3 == 0);
            return thirdDocuments;
        }

        public void CreateAccountDocument(string accountId)
        {
            IEnumerable<string> thirdDocuments = GetAccountAllThirdDocumentsPaths(accountId);

            using var outputStream = new StreamWriter($"Account{accountId}Document.txt");

            foreach (var document in thirdDocuments)
            {
                var documentContent = File.ReadAllText(document);
                if (documentContent.Contains("Smith Property"))
                {
                    outputStream.WriteLine(documentContent);
                    outputStream.WriteLine();
                }
            }
        }

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
