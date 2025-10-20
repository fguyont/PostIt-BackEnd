using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models.Responses
{
    public class GetUserResponse
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
