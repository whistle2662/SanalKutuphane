using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

    }
}