using FlowrSpot.Domain.Entities;

namespace FlowrSpot.Application.Repositories
{
    public interface IFlowerRepository
    {
        Task<IEnumerable<Flower>> GetFlowersAsync();
        Task<Flower?> GetFlowerAsync(Guid id);
        Task AddFlowerAsync(Flower flower);
    }
}
