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
    public class RolesListController : Controller
    {
        private readonly IRoleListService _listService;
        private readonly IUserRoleListService _userRoleService;
        private readonly IUserListService _userService;
        private readonly IMapper _mapper;

        public RolesListController(IRoleListService userListService, IUserRoleListService userRoleService, IUserListService userService, IMapper mapper)
        {
            _listService = userListService;
            _mapper = mapper;
            _userRoleService = userRoleService;
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexRolesList()
        {
            List<ApplicationRole> list = new();
            
            var response = await _listService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ApplicationRole>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> UpdateRolesList(string id)
        //{
        //    var response = await _listService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        ApplicationRole model = JsonConvert.DeserializeObject<ApplicationRole>(Convert.ToString(response.Result));
        //        return View(_mapper.Map<ApplicationRole>(model));
        //    }
        //    return NotFound();
        //}

        //[Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateRolesList(ApplicationRole model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        TempData["success"] = "Typ ogłoszenia został zaktualizowany";
        //        var response = await _listService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
        //        if (response != null && response.IsSuccess)
        //        {
        //            return RedirectToAction(nameof(IndexRolesList));
        //        }
        //    }
        //    TempData["error"] = "Error";
        //    return View(model);
        //}

        //===============================================================
        public async Task<IActionResult> UpdateRolesList(string id)
        {
            EditRoleViewModelVM adVM = new();
            var response = await _listService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ApplicationRole model = JsonConvert.DeserializeObject<ApplicationRole>(Convert.ToString(response.Result));
                adVM.Role = model;
            }

            response = await _userRoleService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response1 = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response1 != null && response.IsSuccess)
            {
                adVM.RoleList = JsonConvert.DeserializeObject<List<ApplicationRole>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                //adVM.UserRole = JsonConvert.DeserializeObject<List<ApplicationUserRole>>
                //    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                //    {
                //        Text = i.RoleId,
                //        Value = i.RoleId.ToString()//  Id.ToString()
                //    });
                return View(adVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRolesList(EditRoleViewModelVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _listService.UpdateAsync<APIResponse>(model.Role, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Rola zostałA zaktualizowana";
                    return RedirectToAction(nameof(IndexRolesList));
                }
            }
            return View(model);
        }


        //===============================================================  END
       

        public async Task<IActionResult> CreateRolesList()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRolesList(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                var response = await _listService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Rola została dodana";
                    return RedirectToAction(nameof(IndexRolesList));
                }
            }
            TempData["error"] = "Error";
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRolesList(string id)
        {
            var response = await _listService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ApplicationRole model = JsonConvert.DeserializeObject<ApplicationRole>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRolesList(ApplicationRole model)
        {
            var response = await _listService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Rola została usunięta";
                return RedirectToAction(nameof(IndexRolesList));
            }
            TempData["error"] = "Error";
            return View(model);
        }
    }
}
