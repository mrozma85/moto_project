using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IImageService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(VehicleImages dto, string token);
        Task<T> UpdateAsync<T>(int id, VehicleImages dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T> GetAllByAdId<T>(int id, VehicleImages dto, string token);     
    }
}
