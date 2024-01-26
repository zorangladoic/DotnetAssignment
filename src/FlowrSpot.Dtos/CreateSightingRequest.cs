using System.ComponentModel.DataAnnotations;

namespace FlowrSpot.Dtos
{
    public class CreateSightingRequest
    {
        [Required]
        public string Longitude { get; set; } = string.Empty;
        [Required]
        public string Latitude { get; set; } = string.Empty;
        [Required]
        public Guid FlowerId { get; set; }
    }
}
