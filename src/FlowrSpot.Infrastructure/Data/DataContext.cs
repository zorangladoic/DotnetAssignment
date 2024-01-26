using FlowrSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowrSpot.Infrastructure.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Flower> Flowers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sighting> Sightings { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasKey(like => new { like.UserId, like.SightingId });

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = new Guid("AA39A862-764B-4797-938F-E562150393F9"),
                        Username = "Bob44",
                        Email = "bob44@yahoo.com",
                        Password = "bob123"
                    },
                    new User
                    {
                        Id = new Guid("7A7782D7-231F-4239-A489-AA0FC53F7012"),
                        Username = "test123",
                        Email = "test123@yahoo.com",
                        Password = "test123"
                    }
                );

            modelBuilder.Entity<Flower>()
                .HasData(
                    new Flower
                    {
                        Id = new Guid("60164C52-E90C-4315-9F4E-95AE1C9F1F03"),
                        Name = "Rose",
                        Description = "Red rose",
                        ImageUrl = "some/imge/rose_url.jpg"
                    },
                    new Flower
                    {
                        Id = new Guid("749778B3-6756-45CC-A5FB-64E8162F5CE8"),
                        Name = "Tulip",
                        Description = "Yellow tulip",
                        ImageUrl = "some/imge/tulip_url.jpg"
                    },
                    new Flower
                    {
                        Id = new Guid("A9537E9E-B3DF-4F91-9EB3-F6BF27026FF0"),
                        Name = "Daffodil",
                        Description = "Yellow daffodil",
                        ImageUrl = "some/imge/daffodil_url.jpg",
                    }
                );

            modelBuilder.Entity<Sighting>()
                .HasData(
                    new Sighting
                    {
                        Id = new Guid("BE561C74-4282-49AF-94A4-5D3E2E146276"),
                        Longitude = "467.345",
                        Latitude = "678,332",
                        FlowerId = new Guid("A9537E9E-B3DF-4F91-9EB3-F6BF27026FF0"),
                        UserId = new Guid("7A7782D7-231F-4239-A489-AA0FC53F7012"),
                    }
                );

            modelBuilder.Entity<Like>()
                .HasData(
                    new Like
                    {
                        SightingId = new Guid("BE561C74-4282-49AF-94A4-5D3E2E146276"),
                        UserId = new Guid("AA39A862-764B-4797-938F-E562150393F9")
                    }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
