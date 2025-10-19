using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models.Responses
{
    public class UserLoginSuccess
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Email { get; set; }

        public required string Token { get; set; }
    }
}
