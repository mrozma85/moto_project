using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IVehicleService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
		Task<T> GetNameSearch<T>(string search, string token);
		Task<T> CreateAsync<T>(VehicleDTO dto, string token);
        Task<T> UpdateAsync<T>(VehicleDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
