using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Syncfusion.XPS;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TESTAPIuserroles : ControllerBase
    {
        private readonly MotoDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TESTAPIuserroles(MotoDbContext db,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _db= db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("{id:Guid}")]
        //[Authorize]
        public async Task<IActionResult> GetUserwithRoles(string id, string roleId, string nameRole)
        {
            var users = _userManager.Users;

            var userId = await _userManager.FindByIdAsync(new Guid(id).ToString()); //identityRole

            var wszyscy = _db.ApplicationUsers.GetAsyncEnumerator();
            var user = _db.ApplicationUsers.Where(u => u.Id == id).FirstOrDefault();

            var roles = await _userManager.GetRolesAsync(userId);

            //dziala usuwanie
            await _userManager.RemoveFromRolesAsync(userId, roles.ToArray());

            //
            await _userManager.AddToRoleAsync(userId, nameRole);
            //await _userManager.AddToRoleByRoleIdAsync(userId, roleId);

            return Ok();
        }
    }
}
