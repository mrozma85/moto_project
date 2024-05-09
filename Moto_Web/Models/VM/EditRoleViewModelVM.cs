using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Web.Models.Dto;

namespace Moto_Web.Models.VM
{
    public class EditRoleViewModelVM
    {
        public EditRoleViewModelVM()
        {
            Role = new ApplicationRole();
        }
        public string Id { get; set; }
        public ApplicationRole Role { get; set; }
       

        public ApplicationUserRole UserRole { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }

    }
}       

