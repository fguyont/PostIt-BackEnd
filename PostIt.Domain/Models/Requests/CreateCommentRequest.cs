using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models.Requests
{
    public class CreateCommentRequest
    {
        [Required]
        [MaxLength(2000)]
        public required string Text { get; set; }
    }
}
