using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.VM;
using Moto_Web.Services.IServices;
using Newtonsoft.Json;
using System.Data;
using Windows.System;

namespace Moto_Web.Controllers
{
    public class UserRolesListController : Controller
    {
        private readonly IUserRoleListService _listService;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRoleListService _userRoleService;

        public UserRolesListController(IRoleListService userRoleService,/* UserManager<ApplicationUser> userManager,*/ IUserRoleListService listService, IMapper mapper)
        {
            _listService = listService;
            _mapper = mapper;
            _userRoleService = userRoleService;
            //_userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexUserRolesList()
        {
            List<ApplicationUserRole> list = new();
            
            var response = await _listService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ApplicationUserRole>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUserRolesList(string id, Roles roleName)
        {
            //var userId = await _userManager.FindByIdAsync(new Guid(id).ToString());
            //var roles = await _userManager.GetRolesAsync(userId);

            EditRoleUserViewModelVM adVM = new();
            var response = await _listService.GetAsync<APIResponse>(id, roleName, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ApplicationUserRole model = JsonConvert.DeserializeObject<ApplicationUserRole>(Convert.ToString(response.Result));
                //adVM.Roles = roleName;
                
                adVM.UserRole = model;
            }

            response = await _userRoleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                adVM.RoleList = JsonConvert.DeserializeObject<List<ApplicationRole>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        //Value = i.Id.ToString()
                        Value = i.Name.ToString()
                    });
                return View(adVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserRolesList(string roleName, EditRoleUserViewModelVM adVM)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "UserRole został zaktualizowany";
                var response = await _listService.UpdateAsync<APIResponse>(adVM.Id, roleName, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexUserRolesList));
                }
            }
            TempData["error"] = "Error";
            return View(adVM);
        }

        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteUserRolesList(string id)
        //{
        //    var response = await _listService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        ApplicationUserRole model = JsonConvert.DeserializeObject<ApplicationUserRole>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserRolesList(ApplicationUserRole model)
        {
            var response = await _listService.DeleteAsync<APIResponse>(model.UserId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "UserRole został usunięty";
                return RedirectToAction(nameof(IndexUserRolesList));
            }
            TempData["error"] = "Error";
            return View(model);
        }
    }
}
