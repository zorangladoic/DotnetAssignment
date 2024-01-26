using FlowrSpot.Domain.Entities;

namespace FlowrSpot.Application.Repositories
{
    public interface ILikeRepository
    {
        Task<Like?> GetLikeByIdAsync(Guid sightingId, Guid userId);
        Task<IEnumerable<Like>> GetLikesBySightingIdAsync(Guid sightingId);
        Task<int> GetSightingLikeCounterAsync(Guid sightingId);
        Task AddLikeAsync(Like like);
        Task DeleteLikeAsync(Like like);
    }
}
