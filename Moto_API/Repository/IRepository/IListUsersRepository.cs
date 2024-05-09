using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Models;
using Moto_API.Models.Dto;
using System.Linq.Expressions;

namespace Moto_API.Repository.IRepository
{
    public interface IListUsersRepository : IRepository<ApplicationUser>
    {
        Task<List<ApplicationUser>> UpdateAsync(string roleName, List<ApplicationUser> entity);
        Task<List<ApplicationUser>> GetById(string id);
    }
}
