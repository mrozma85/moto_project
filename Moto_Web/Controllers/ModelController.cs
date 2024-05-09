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
using System.Security.Claims;
using Windows.Services.Maps;

namespace Moto_Web.Controllers
{
    public class ModelController : Controller
    {
        private readonly IModelService _modelService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public ModelController(IModelService modelService, ICompanyService companyService, IMapper mapper)
        {
            _modelService = modelService;
            _companyService = companyService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexModel()
        {
            List<Model> list = new();

            var response = await _modelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<Model>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateModel()
        {
            ModelVM modelVM = new();

            var response1 = await _companyService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var response2 = await _modelService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response1 != null && response2!= null && response1.IsSuccess  && response2.IsSuccess)
            {
                modelVM.CompanyList = JsonConvert.DeserializeObject<List<Company>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(modelVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModel(ModelVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _modelService.CreateAsync<APIResponse>(model.ModelClass, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Model został dodany";
                    return RedirectToAction(nameof(IndexModel));
                }
            }
            TempData["error"] = "Error";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateModel(int id)
        {
            ModelVM modelVM = new();
            var response = await _modelService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                Model model = JsonConvert.DeserializeObject<Model>(Convert.ToString(response.Result));
                modelVM.ModelClass = model;
            }

            var response1 = await _companyService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response1 != null && response1.IsSuccess)
            {
                modelVM.CompanyList = JsonConvert.DeserializeObject<List<Company>>
                    (Convert.ToString(response1.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(modelVM);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateModel(ModelVM model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Model został zaktualizowany";
                var response = await _modelService.UpdateAsync<APIResponse>(model.ModelClass, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexModel));
                }
            }
            TempData["error"] = "Error";
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var response = await _modelService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                Model model = JsonConvert.DeserializeObject<Model>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteModel(Model model)
        {
            var response = await _modelService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Model został usunięty";
                return RedirectToAction(nameof(IndexModel));
            }
            TempData["error"] = "Error";
            return View(model);
        }
    }
}
