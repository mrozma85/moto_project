using Microsoft.AspNetCore.Identity;
using Moto_API.Models;
using System.Linq.Expressions;

namespace Moto_API.Repository.IRepository
{
    public interface IListUserRolesRepository : IRepository<ApplicationUserRole>
    {
        Task<ApplicationUserRole> UpdateAsync(ApplicationUserRole entity);
        Task<bool> Update1111Async(string userId, string roleName);
    }
}
