using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Web.Models.Dto;

namespace Moto_Web.Models.VM
{
    public class AdUpdateVM
    {
        public AdUpdateVM()
        {
            Ad = new AdUpdateDTO();
        }

        public AdUpdateDTO Ad { get; set; }
        public int VehicleId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> AdTypeList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public List<Company> Company { get; set; }
        public List<Model> Model { get; set; }
    }
}
