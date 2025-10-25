using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models.Requests
{
    public class CreateUpdatePostRequest
    {
        [Required]
        [MaxLength(50)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(5000)]
        public required string Text { get; set; }
    }
}
