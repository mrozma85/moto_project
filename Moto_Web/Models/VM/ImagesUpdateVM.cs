using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Web.Models.Dto;

namespace Moto_Web.Models.VM
{
    public class ImagesUpdateVM
    {
        public ImagesUpdateVM()
        {
            VehicleImages = new List<VehicleImages>();
        }

        public List<VehicleImages> VehicleImages { get; set; }
    }
}
