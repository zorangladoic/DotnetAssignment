using FlowrSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowrSpot.Infrastructure.Data
{
    internal interface IDataContext
    {
        DbSet<Flower> Flowers { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Sighting> Sightings { get; set; }
        DbSet<Like> Likes { get; set; }
    }
}
