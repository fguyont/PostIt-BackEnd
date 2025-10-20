using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostIt.Database.EntityModels
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public virtual required Subject Subject { get; set; }

        public long UserId { get; set; }

        public virtual required User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
