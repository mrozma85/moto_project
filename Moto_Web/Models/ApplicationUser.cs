using Microsoft.AspNetCore.Identity;

namespace Moto_Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
