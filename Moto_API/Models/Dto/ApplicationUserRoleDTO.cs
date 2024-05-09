using Microsoft.AspNetCore.Identity;

namespace Moto_API.Models.Dto
{
    public class ApplicationUserRoleDTO : IdentityUserRole<string>
    {
        public virtual ApplicationRole Role { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
