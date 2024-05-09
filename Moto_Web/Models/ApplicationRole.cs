using Microsoft.AspNetCore.Identity;

namespace Moto_Web.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
