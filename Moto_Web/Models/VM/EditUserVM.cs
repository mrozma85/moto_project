using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Moto_Web.Models.VM
{
    public class EditUserVM
    {
        public EditUserVM()
        {
            User = new ApplicationUser();
            Role = new ApplicationRole();
        }

        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> UserRoleList { get; set; }
    }
}
