using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IMainPageDetailsService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> CreateAsync<T>(MainPageDetails dto, string token);
        Task<T> UpdateAsync<T>(MainPageDetails dto, string token);
    }
}
