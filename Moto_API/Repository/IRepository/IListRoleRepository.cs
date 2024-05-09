using Microsoft.AspNetCore.Identity;
using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface IListRoleRepository : IRepository<ApplicationRole>
    {
        Task<ApplicationRole> UpdateAsync(ApplicationRole entity);
        Task<ApplicationRole> CreateNewTest(ApplicationRole entity);
        Task<bool> CreateNewTest1(ApplicationRole entity);
    }
}
