using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface ICompanyService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(Company dto, string token);
        Task<T> UpdateAsync<T>(Company dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
