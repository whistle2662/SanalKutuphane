using System.ComponentModel.DataAnnotations;

    namespace WebApplication2.Models
    {
        public class ForgotPasswordViewModel
        {
            [Required]
            [Display(Name = "Kullanıcı Adı veya E-posta")]
            public string UserNameOrEmail { get; set; }

            [Required]
            [Display(Name = "Güvenlik Sorusu")]
            public string SecurityQuestion { get; set; }

            [Required]
            [Display(Name = "Güvenlik Sorusu Cevabı")]
            public string SecurityAnswer { get; set; }
        }
    }
