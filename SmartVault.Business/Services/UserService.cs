using SmartVault.Business.Services.Interfaces;
using SmartVault.Data.Repositories.Interfaces;

namespace SmartVault.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int GetCount() => _userRepository.GetCount();
    }
}
