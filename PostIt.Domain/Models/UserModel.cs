using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models
{
    public class UserModel
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public ICollection<long> PostIds { get; set; } = new List<long>();

        public ICollection<long> CommentIds { get; set; } = new List<long>();

        public ICollection<long> SubjectIds { get; set; } = new List<long>();
    }
}
