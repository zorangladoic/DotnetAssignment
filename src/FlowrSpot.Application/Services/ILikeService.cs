using Ardalis.Result;
using FlowrSpot.Dtos;

namespace FlowrSpot.Application.Services
{
    public interface ILikeService
    {
        Task<Result<LikeDto>> CreateLikeAsync(Guid sightingId, string loggedInUser);
        Task<bool> DeleteLikeAsync(Guid sightingId, string loggedInUser);
    }
}
