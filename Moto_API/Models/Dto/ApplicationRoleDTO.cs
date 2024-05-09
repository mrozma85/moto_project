 using Microsoft.AspNetCore.Identity;

namespace Moto_API.Models
{
    public class ApplicationRoleDTO : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
