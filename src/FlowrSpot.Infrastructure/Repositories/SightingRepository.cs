using FlowrSpot.Application.Repositories;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowrSpot.Infrastructure.Repositories
{
    public class SightingRepository : ISightingRepository
    {
        private readonly DataContext _context;

        public SightingRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sighting>> GetSightingsAsync()
        {
            return await _context.Sightings.ToListAsync();
        }

        public async Task<Sighting?> GetSightingAsync(Guid id)
        {
            return await _context.Sightings.FirstOrDefaultAsync(sighting => sighting.Id == id);
        }

        public async Task AddSightingAsync(Sighting sighting)
        {
            await _context.Sightings.AddAsync(sighting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSightingAsync(Sighting sighting)
        {
            _context.Sightings.Remove(sighting);
            await _context.SaveChangesAsync();

        }
    }
}
