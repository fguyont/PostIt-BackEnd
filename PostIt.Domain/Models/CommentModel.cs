using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models
{
    public class CommentModel
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(2000)]
        public required string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public required long PostId { get; set; }

        public required string PostTitle { get; set; }

        public required long UserId { get; set; }

        public required string UserName { get; set; }
    }
}
