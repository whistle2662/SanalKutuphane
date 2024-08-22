using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

}