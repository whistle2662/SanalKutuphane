using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi Onayla")]
        [Compare("Password", ErrorMessage = "Şifre ve onay şifresi eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Güvenlik Sorusu")]
        public string SecurityQuestion { get; set; }

        [Required]
        [Display(Name = "Güvenlik Sorusu Cevabı")]
        public string SecurityAnswer { get; set; }
    }
}
