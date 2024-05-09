using Microsoft.AspNetCore.Identity;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class ListRoleRepository : Repository<ApplicationRole>, IListRoleRepository
    {
        private readonly MotoDbContext _db;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ListRoleRepository(MotoDbContext db, RoleManager<ApplicationRole> roleManager) : base(db)
        {
            _db = db;
            _roleManager = roleManager;

        }

        //public async Task<IdentityRole> UpdateAsync(IdentityRole entity) ????????????
        public async Task<ApplicationRole> UpdateAsync(ApplicationRole entity)
        {
            _db.Roles.Update(entity);
            //_db.ApplicationRoles.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<ApplicationRole> CreateNewTest(ApplicationRole entity)
        {
            //_db.Roles.Add(entity);
            //_db.ApplicationRoles.Add(entity);
            await _roleManager.CreateAsync(entity);
            //await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> CreateNewTest1(ApplicationRole entity)
        {
            //_db.Roles.Add(entity);
            //_db.ApplicationRoles.Add(entity);
            await _roleManager.CreateAsync(entity);
            //await _db.SaveChangesAsync();
            return true;
        }
    }
}
