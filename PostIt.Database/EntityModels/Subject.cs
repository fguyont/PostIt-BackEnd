using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostIt.Database.EntityModels
{
    [Table("Subjects")]
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
