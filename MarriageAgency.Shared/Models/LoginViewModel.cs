using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Shared.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Имя пользователя слишком длинное!")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Пароль слишком длинный!")]
        public string Password { get; set; }
    }
}