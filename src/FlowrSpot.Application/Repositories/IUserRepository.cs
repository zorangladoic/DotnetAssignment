using FlowrSpot.Domain.Entities;

namespace FlowrSpot.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username);
        Task<User?> GetUserAsync(Guid id);
        Task AddUserAsync(User user);
        Task<bool> IsUsernameUnique(string username);
        Task<bool> IsEmailUnique(string email);

    }
}
