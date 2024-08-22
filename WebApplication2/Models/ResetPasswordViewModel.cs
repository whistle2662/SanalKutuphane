using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ResetPasswordViewModel
    {
        public string UserNameOrEmail { get; set; }
        [Required]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Yeni Şifreyi Onayla")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifre ve onay şifresi eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }


    }
}
