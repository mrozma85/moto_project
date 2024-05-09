using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IRoleListService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAllAsyncName<T>(string token);
        Task<T> GetAsync<T>(string id, string token);
        Task<T> CreateAsync<T>(ApplicationRole dto, string token);
        Task<T> UpdateAsync<T>(ApplicationRole dto, string token);
        Task<T> DeleteAsync<T>(string id, string token);
    }
}
