using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models.Responses
{
    public class UserLoginSuccess
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
