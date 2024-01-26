
namespace FlowrSpot.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<Sighting> Sightings { get; set; } = new List<Sighting>();
        public IList<Like> Likes { get; set; } = new List<Like>();
    }
}
