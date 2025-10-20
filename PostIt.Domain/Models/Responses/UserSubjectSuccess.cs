using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models.Responses
{
    public class UserSubjectSuccess
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public required string UserName { get; set; }

        [Required]
        public long SubjectId { get; set; }

        [Required]
        public required string SubjectName { get; set; }
    }
}
