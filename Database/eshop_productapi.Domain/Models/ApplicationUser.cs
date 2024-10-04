using Microsoft.AspNetCore.Identity;

namespace eshop_productapi.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public System.Guid? PasswordReset { get; set; }
        public System.DateTime? PasswordResetDate { get; set; }
    }
}