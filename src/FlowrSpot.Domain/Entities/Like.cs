
namespace FlowrSpot.Domain.Entities
{
    public class Like
    {
        public Guid UserId { get; set; }
        public Guid SightingId { get; set; }
        public User User { get; set; } = default!;
        public Sighting Sighting { get; set; } = default!;
    }
}
