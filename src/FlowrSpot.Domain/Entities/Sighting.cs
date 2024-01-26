
namespace FlowrSpot.Domain.Entities
{
    public class Sighting
    {
        public Guid Id { get; set; }
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid FlowerId { get; set; }
        public User User { get; set; } = default!;
        public Flower Flower { get; set; } = default!;
        public IList<Like> Likes { get; set; } = new List<Like>();
    }
}
