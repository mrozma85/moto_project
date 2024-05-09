using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Web.Models.Dto;

namespace Moto_Web.Models.VM
{
    public class EditRoleUserViewModelVM
    {
        public EditRoleUserViewModelVM()
        {
            UserRole = new ApplicationUserRole();
        }
        public string Id { get; set; }
        public string RoleName { get; set; }

        public ApplicationUserRole UserRole { get; set; }
        public List<Roles> Roles { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }

    }
}       

