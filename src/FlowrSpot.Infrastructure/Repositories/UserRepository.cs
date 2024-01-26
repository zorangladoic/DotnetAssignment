using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowrSpot.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task<User?> GetUserAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<bool> IsUsernameUnique(string username)
        {
            return !await _context.Users.Where(user => user.Username == username).AnyAsync();
        }
        public async Task<bool> IsEmailUnique(string email)
        {
            return !await _context.Users.Where(user => user.Email.ToLower() == email.ToLower()).AnyAsync();
        }
    }
}
