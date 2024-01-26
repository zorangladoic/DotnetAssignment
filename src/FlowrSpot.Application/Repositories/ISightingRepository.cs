using FlowrSpot.Domain.Entities;

namespace FlowrSpot.Application.Repositories
{
    public interface ISightingRepository
    {
        Task<IEnumerable<Sighting>> GetSightingsAsync();
        Task<Sighting?> GetSightingAsync(Guid id);
        Task DeleteSightingAsync(Sighting sighting);
        Task AddSightingAsync(Sighting sighting);
    }
}
