using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models.Requests
{
    public class CreateSubjectRequest
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Description { get; set; }
    }
}
