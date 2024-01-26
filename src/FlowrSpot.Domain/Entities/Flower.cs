
namespace FlowrSpot.Domain.Entities
{
    public class Flower
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public IList<Sighting> Sighting { get; set; } = new List<Sighting>();
    }
}
