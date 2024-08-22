using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ResetPasswordBySecurityQuestionViewModel
    {
        [Required]
        public string UserNameOrEmail { get; set; }

        [Required]
        public string SecurityQuestion { get; set; }

        [Required]
        public string SecurityAnswer { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }

}
