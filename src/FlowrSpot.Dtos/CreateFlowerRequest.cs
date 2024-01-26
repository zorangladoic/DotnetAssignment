using System.ComponentModel.DataAnnotations;

namespace FlowrSpot.Dtos
{
    public class CreateFlowerRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
