using System.ComponentModel.DataAnnotations;

namespace FlowrSpot.Dtos
{
    public class CreateLikeRequest
    {
        [Required]
        public Guid SightingId { get; set; }
    }
}
