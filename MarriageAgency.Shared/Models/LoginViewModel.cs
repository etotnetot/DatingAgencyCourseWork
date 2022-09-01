using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Shared.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "User name is too long!")]
        public string Username { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Password is too long!")]
        public string Password { get; set; }
    }
}