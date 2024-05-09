using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Web.Models.Dto;

namespace Moto_Web.Models.VM
{
    public class AdCreateVM
    {
        public AdCreateVM()
        {
            Ad = new AdCreateDTO();
        }

        public AdCreateDTO Ad { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> AdTypeList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ModelList { get; set; }


        public List<Company> Company { get; set; }
        public List<Model> Model { get; set; }
    }
}
