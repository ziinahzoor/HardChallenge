using SmartVault.Business.Services.Interfaces;
using SmartVault.Data.Repositories.Interfaces;

namespace SmartVault.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public int GetCount() => _accountRepository.GetCount();
    }
}
