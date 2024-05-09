using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IModelService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(Model dto, string token);
        Task<T> UpdateAsync<T>(Model dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
