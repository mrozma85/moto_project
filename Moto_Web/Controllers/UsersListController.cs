using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Models.VM;
using Moto_Web.Services;
using Moto_Web.Services.IServices;
using Newtonsoft.Json;
using System.Data;

namespace Moto_Web.Controllers
{
    public class UsersListController : Controller
    {
        private readonly IUserListService _listService;
        private readonly IRoleListService _RolesService;
        private readonly IMapper _mapper;

        public UsersListController(IUserListService userListService, IMapper mapper, IRoleListService RolesService)
        {
            _listService = userListService;
            _mapper = mapper;
            _RolesService = RolesService;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexUsersList()
        {
            List<ApplicationUser> list = new();
            
            var response = await _listService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ApplicationUser>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUsersList(string id)
        {
            UsersUpdateVM userUpdateVM = new();
            var response1 = await _listService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response1 != null && response1.IsSuccess)
            {
                List<ApplicationUser> model = JsonConvert.DeserializeObject<List<ApplicationUser>>(Convert.ToString(response1.Result));
                userUpdateVM.User = model;
            }

            var response2 = await _RolesService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response2 != null && response2.IsSuccess)
            {
                userUpdateVM.RoleList = JsonConvert.DeserializeObject<List<ApplicationRole>>
                    (Convert.ToString(response2.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Name.ToString()
                    });

                var tempRoles = new List<ApplicationUserRole>();
                var lista = userUpdateVM.User[0].UserRoles;

                foreach (var item in lista)
                {
                    tempRoles.Add(item);
                }

                userUpdateVM.Role = tempRoles;
                return View(userUpdateVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUsersList(UsersUpdateVM model, string roleName)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Dane użytkownika zostały zaktualizowany";
                var name = model.Role[0].Role.Name;
                roleName = name;
                var response = await _listService.UpdateAsync<APIResponse>(model.User, roleName, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexUsersList));
                }
            }
            TempData["error"] = "Error";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUsersList(string id)
        {
            var response = await _listService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ApplicationUser model = JsonConvert.DeserializeObject<ApplicationUser>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsersList(ApplicationUser model)
        {
            var response = await _listService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Dane użytkownika zostały usunięte";
                return RedirectToAction(nameof(IndexUsersList));
            }
            TempData["error"] = "Error";
            return View(model);
        }
    }
}
