using AutoMapper;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Models.Dto.Category;

namespace Moto_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            //CreateMap<Vehicle, VehicleDTO>();
            //CreateMap<VehicleDTO, Vehicle>();
            // Zamiast powyzej jedna linia z Revers
            CreateMap<Vehicle, VehicleDTO>().ReverseMap();
            CreateMap<Ad, AdDTO>().ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();
			CreateMap<Category, CategoryCreateDTO>().ReverseMap();
			CreateMap<Category, CategoryUpdateDTO>().ReverseMap();

			CreateMap<AdType, AdTypeDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
            CreateMap<ApplicationUserRole, ApplicationUserRoleDTO>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleDTO>().ReverseMap();
        }
    }
}
