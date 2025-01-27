using SmartVault.Business.Services.Interfaces;
using SmartVault.Data.Repositories.Interfaces;

namespace SmartVault.Business.Services
{
    public class OAuthUserService : IOAuthUserService
    {
        private readonly IOAuthUserRepository _userRepository;

        public OAuthUserService(IOAuthUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int GetCount() => _userRepository.GetCount();
    }
}
