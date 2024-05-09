using Moto_Web.Models;

namespace Moto_Web.Services.IServices
{
    public interface IUserListService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(string id, string token);
        //Task<T> CreateAsync<T>(ApplicationUser dto, string token);
        Task<T> UpdateAsync<T>(List<ApplicationUser> dto, string roleName, string token);
        Task<T> DeleteAsync<T>(string id, string token);
    }
}
