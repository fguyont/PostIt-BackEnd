using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models
{
    public class SubjectModel
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Description { get; set; }

        public ICollection<long> PostIds { get; set; } = new List<long>();

        public ICollection<long> UserIds { get; set; } = new List<long>();
    }
}
