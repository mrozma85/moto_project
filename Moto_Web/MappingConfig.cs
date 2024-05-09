using AutoMapper;
using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Vehicle, VehicleDTO>().ReverseMap();

			//CreateMap<ApplicationUserDTO, ApplicationUserDTO>().ReverseMap();

		    CreateMap<Ad, AdDTO>().ReverseMap();
			CreateMap<AdDTO, AdNameDTO>().ReverseMap();
			CreateMap<AdDTO, AdCreateDTO>().ReverseMap();
            CreateMap<AdDTO, AdUpdateDTO>().ReverseMap();
            CreateMap<AdDTO, AdDeleteDTO>().ReverseMap();

            //CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CategoryDTO, CategoryCreateDTO>().ReverseMap();
            CreateMap<CategoryDTO, CategoryUpdateDTO>().ReverseMap();

            //CreateMap<AdType, AdTypeDTO>().ReverseMap();
            CreateMap<AdTypeDTO, AdTypeCreateDTO>().ReverseMap();
            CreateMap<AdTypeDTO, AdTypeUpdateDTO>().ReverseMap();
		}
    }
}
