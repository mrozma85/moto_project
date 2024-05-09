using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Web.Models.Dto;
using Windows.System;

namespace Moto_Web.Models.VM
{
    public class DetailsVehicleVM
	{
        public DetailsVehicleVM()
        {
			Ad = new List<AdDTO>();
		}

		public List<AdDTO> Ad { get; set; }
        public List<AdDTO> AdName { get; set; }

	}
}
