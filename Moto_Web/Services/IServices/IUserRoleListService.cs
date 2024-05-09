using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IUserRoleListService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(string id, Roles roleName, string token);
        //Task<T> CreateAsync<T>(ApplicationUserRole dto, string token);
        Task<T> UpdateAsync<T>(string id, string roleName, /*ApplicationUserRole dto, */string token);
        Task<T> DeleteAsync<T>(string id, string token);
    }
}
