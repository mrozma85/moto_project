using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Moto_API.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationRole Role { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
