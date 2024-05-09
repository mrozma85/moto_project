using Microsoft.AspNetCore.Identity;

namespace Moto_Web.Models
{
    public class ApplicationUserUpdate : IdentityUser
    {
        public string Name { get; set; }
        public List<ApplicationUserRole> Roles { get; set; }
        //public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
