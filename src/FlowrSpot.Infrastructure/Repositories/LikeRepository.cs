using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowrSpot.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Like?> GetLikeByIdAsync(Guid sightingId, Guid userId)
        {
            return await _context.Likes
                .FirstOrDefaultAsync(like => like.SightingId == sightingId && like.UserId == userId);
        }

        public async Task<IEnumerable<Like>> GetLikesBySightingIdAsync(Guid sightingId)
        {
            return await _context.Likes.Where(like => like.SightingId == sightingId).ToListAsync();
        }
        public async Task AddLikeAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLikeAsync(Like like)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetSightingLikeCounterAsync(Guid sightingId)
        {
            return await _context.Likes.CountAsync(like => like.SightingId == sightingId);
        }
    }
}
