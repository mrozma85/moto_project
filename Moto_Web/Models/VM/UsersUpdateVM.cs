using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Moto_Web.Models.VM
{
    public class UsersUpdateVM
    {
        public UsersUpdateVM()
        {
            User = new List<ApplicationUser>();
        }

        //public List<ApplicationUser> User { get; set; }
        public List<ApplicationUser> User { get; set; }
        public List<ApplicationUserRole> Role { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
