using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Web.Models.Dto;

namespace Moto_Web.Models.VM
{
    public class AdDeleteVM
    {
        public AdDeleteVM()
        {
            Ad = new AdDeleteDTO();
        }

        public AdDeleteDTO Ad { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> AdTypeList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
