using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class ListUserRolesRepository : Repository<ApplicationUserRole>, IListUserRolesRepository
    {
        private readonly MotoDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ListUserRolesRepository(MotoDbContext db, UserManager<ApplicationUser> userManager) : base(db)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<ApplicationUserRole> UpdateAsync(ApplicationUserRole entity)
        {
            //_db.UserRoles.Update(entity);
            _db.ApplicationUserRoles.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Update1111Async(string user, string nameRole)
        {
            var userId = await _userManager.FindByIdAsync(new Guid(user).ToString());
            var roles = await _userManager.GetRolesAsync(userId);

            //dziala usuwanie
            await _userManager.RemoveFromRolesAsync(userId, roles.ToArray());
            //
            await _userManager.AddToRoleAsync(userId, nameRole);
            //await _userManager.AddToRoleByRoleIdAsync(userId, roleId);

            return true;
        }
    }
}
