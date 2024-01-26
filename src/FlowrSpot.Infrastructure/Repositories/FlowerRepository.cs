using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowrSpot.Infrastructure.Repositories
{
    public class FlowerRepository : IFlowerRepository
    {
        private readonly DataContext _context;

        public FlowerRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Flower>> GetFlowersAsync()
        {
            return await _context.Flowers.ToListAsync();
        }

        public async Task<Flower?> GetFlowerAsync(Guid id)
        {
            return await _context.Flowers.FirstOrDefaultAsync(flower => flower.Id == id);
        }

        public async Task AddFlowerAsync(Flower flower)
        {
            await _context.Flowers.AddAsync(flower);
            await _context.SaveChangesAsync();
        }
    }
}
