using System.ComponentModel.DataAnnotations;

namespace FlowrSpot.Dtos
{
    public class DeleteLikeRequest
    {
        [Required]
        public Guid SightingId { get; set; }
    }
}
