using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IAdTypeService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(AdTypeCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(AdTypeUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
