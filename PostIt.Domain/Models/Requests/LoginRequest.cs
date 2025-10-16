using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
