using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models
{
    public class PostModel
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Title { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        [MaxLength(5000)]
        public required string Text { get; set; }

        public bool IsActive { get; set; }

        public long SubjectId { get; set; }

        public required string SubjectName { get; set; }

        public long UserId { get; set; }

        public required string UserName { get; set; }
    }
}
