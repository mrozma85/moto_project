using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services;
using Moto_Web.Services.IServices;
using Newtonsoft.Json;
using System.Data;

namespace Moto_Web.Controllers
{
    public class AdTypesController : Controller
    {
        private readonly IAdTypeService _adTypeService;
        private readonly IMapper _mapper;

        public AdTypesController(IAdTypeService adTypeService, IMapper mapper)
        {
            _adTypeService = adTypeService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexAdTypes()
        {
            List<AdTypeDTO> list = new();

            var response = await _adTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<AdTypeDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAdTypes()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdTypes(AdTypeCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _adTypeService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Typ ogłoszenia został dodany";
                    return RedirectToAction(nameof(IndexAdTypes));
                }
            }
            TempData["error"] = "Error";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAdTypes(int id)
        {
            var response = await _adTypeService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                AdTypeDTO model = JsonConvert.DeserializeObject<AdTypeDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<AdTypeUpdateDTO>(model));
            }
            return NotFound();
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdTypes(AdTypeUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Typ ogłoszenia został zaktualizowany";
                var response = await _adTypeService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexAdTypes));
                }
            }
            TempData["error"] = "Error";
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAdTypes(int id)
        {
            var response = await _adTypeService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                AdTypeDTO model = JsonConvert.DeserializeObject<AdTypeDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdTypes(AdTypeDTO model)
        {
            var response = await _adTypeService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Typ ogłoszenia został usunięty";
                return RedirectToAction(nameof(IndexAdTypes));
            }
            TempData["error"] = "Error";
            return View(model);
        }
    }
}
