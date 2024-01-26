using FlowrSpot.Domain.Entities;
using Ardalis.Result;
using FlowrSpot.Dtos;

namespace FlowrSpot.Application.Services
{
    public interface ISightingService
    {
        Task<Result<CreateSightingDto>> CreateSightingAsync(Sighting sighting, string username, string apiSercret);
        Task DeleteSightingAsync(Sighting sighting);
        Task<Result<SightingDto>> GetSightingAsync(Guid id);
        Task<IEnumerable<SightingDto>> GetSightingsAsync();
        Task<bool> IsSightingCreatedByUser(Sighting sighting, string username);
    }
}
