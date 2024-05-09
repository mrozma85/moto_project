using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Models.VM;
using Moto_Web.Services.IServices;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace Moto_Web.Controllers
{
    public class AdsController : Controller
    {
        private readonly IAdService _adService;
        private readonly IAdTypeService _adTypeService;
        private readonly ICategoryService _categoryService;
        private readonly ICompanyService _companyService;
        private readonly IModelService _modelService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdsController(IAdService adService, IModelService modelService, IMapper mapper, ICompanyService companyService, IAdTypeService adTypeService, ICategoryService categoryService, IWebHostEnvironment hostEnvironment)
        {
            _adService = adService;
            _mapper = mapper;
            _adTypeService = adTypeService;
            _categoryService = categoryService;
            _companyService = companyService;
            _modelService = modelService;
            _hostEnvironment = hostEnvironment;
        }

        [Authorize]
        public async Task<IActionResult> IndexAds()
        {
            List<AdDTO> list = new();
			var response = await _adService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
				list = JsonConvert.DeserializeObject<List<AdDTO>>(Convert.ToString(response.Result));
                }
                return View(list);
        }

        public async Task<IActionResult> UserIdList(string user)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            user = userId.Value;

            List<AdDTO> list = new();
            var response = await _adService.GetAsyncByUser<APIResponse>(user, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<AdDTO>>(Convert.ToString(response.Result));
            }
            var myAds = list.Where(u => u.ApplicationUserId == user);
			List<AdDTO> listUser = new();
			foreach (var ad in myAds)
            {
                listUser.Add(ad);
            }
            return View(listUser);
        }

        

        [Authorize]
        public async Task<IActionResult> CreateAds()
        {
            AdCreateVM adVM = new();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            adVM.Ad.ApplicationUserId = userId.Value;

            var response = await _adTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response1 = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response2 = await _companyService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response3 = await _modelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response1 != null && response2 != null && response3 != null && response.IsSuccess)
            {
                adVM.AdTypeList = JsonConvert.DeserializeObject<List<AdTypeDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                adVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>> 
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            var model = new List<Model>();
            model = JsonConvert.DeserializeObject<List<Model>>(Convert.ToString(response3.Result));
            adVM.Model = model;
            var companies = new List<Company>();
            companies = JsonConvert.DeserializeObject<List<Company>>(Convert.ToString(response2.Result));

            adVM.Company = companies;
            ViewBag.Companies = new SelectList(companies, "Id", "Name");

            var models = new List<Model>();                  
            ViewBag.Models = new SelectList(models, "Id", "Name");

            return View(adVM);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAds(AdCreateVM model)
        {
            if (ModelState.IsValid)
            {
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    model.Ad.ApplicationUserId = userId.Value;

                var response = await _adService.CreateAsync<APIResponse>(model.Ad, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    //TempData["success"] = "Twoje ogłoszenie zostało dodane";
                    HttpContext.Session.SetInt32("adid", response.adId);
                    return RedirectToAction("Index", "Images"/*, response.adId*/);
                    //return RedirectToAction(nameof(IndexAds));
                }
            }
            return View(model);
        }

        public async Task<JsonResult> GetModelByCompanyId(int companyId)
        {
            List<Model> models = new();
            var response = await _modelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                models = JsonConvert.DeserializeObject<List<Model>>(Convert.ToString(response.Result));
            }
            return Json(models.Where(m => m.CompanyId == companyId).ToList());
        }

        public async Task<IActionResult> UpdateAds(int id)
        {
            AdUpdateVM adVM = new();
            var response = await _adService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                AdDTO model = JsonConvert.DeserializeObject<AdDTO>(Convert.ToString(response.Result));
                adVM.Ad = _mapper.Map<AdUpdateDTO>(model);
            }

            response = await _adTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response1 = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response2 = await _companyService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response3 = await _modelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response1 != null && response2 != null && response3 != null && response.IsSuccess)
            {
                adVM.AdTypeList = JsonConvert.DeserializeObject<List<AdTypeDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                adVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                var model = new List<Model>();
                model = JsonConvert.DeserializeObject<List<Model>>(Convert.ToString(response3.Result));
                adVM.Model = model;
                var companies = new List<Company>();
                companies = JsonConvert.DeserializeObject<List<Company>>(Convert.ToString(response2.Result));

                adVM.Company = companies;
                ViewBag.Companies = new SelectList(companies, "Id", "Name");

                var models = new List<Model>();
                ViewBag.Models = new SelectList(models, "Id", "Name");

                var vehId = adVM.Ad.Vehicle.Id;
                adVM.VehicleId = vehId;
                return View(adVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAds(AdUpdateVM model)
        {
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                model.Ad.ApplicationUserId = userId.Value;
                var vehId = model.Ad.VehicleId;
                model.Ad.VehicleId = vehId;

                var response = await _adService.UpdateAsync<APIResponse>(model.Ad, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    //TempData["success"] = "Twoje ogłoszenie zostało zaktualizowane";
                    
                    HttpContext.Session.SetInt32("adid", model.Ad.Id);
                    //return RedirectToAction(nameof(IndexAds));
                    return RedirectToAction("UploadImage", "Images");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteAds(int id)
        {
            AdDeleteVM adVM = new();
            var response = await _adService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                AdDTO model = JsonConvert.DeserializeObject<AdDTO>(Convert.ToString(response.Result));
                adVM.Ad = _mapper.Map<AdDeleteDTO>(model);
            }

            response = await _adTypeService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response1 = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response1 != null && response.IsSuccess)
            {
                adVM.AdTypeList = JsonConvert.DeserializeObject<List<AdTypeDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                adVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(adVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAds(AdDeleteVM model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            model.Ad.ApplicationUserId = userId.Value;
            var response = await _adService.DeleteAsync<APIResponse>(model.Ad.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Twoje ogłoszenie zostało usunięte";
                return RedirectToAction(nameof(IndexAds));
            }
            return View(model);
        }
    }
}
