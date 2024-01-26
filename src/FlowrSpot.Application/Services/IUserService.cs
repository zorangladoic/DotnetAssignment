using FlowrSpot.Domain.Entities;

namespace FlowrSpot.Application.Services
{
    public interface IUserService
    {
        Task<User?> Authenticate(string username, string password);
    }
}
