using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;

namespace FlowrSpot.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            var user = await _userRepository.GetUserAsync(username);

            if ((user == null) || (user.Password != password))
            {
                return null;
            }

            return user;
        }
    }
}
