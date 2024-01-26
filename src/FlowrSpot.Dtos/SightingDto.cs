
namespace FlowrSpot.Dtos
{
    public class SightingDto
    {
        public Guid Id { get; init; }
        public string Longitude { get; init; } = string.Empty;
        public string Latitude { get; init; } = string.Empty;
        public Guid UserId { get; init; }
        public Guid FlowerId { get; init; }
        public string ImageUrl { get; set; } = string.Empty;
        public int LikeCounter { get; set; }
    }
}
