using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Models.VM;
using Moto_Web.Services.IServices;
using Newtonsoft.Json;

namespace Moto_Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexCategory()
        {
            List<CategoryDTO> list = new();

            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

		public async Task<IActionResult> CreateCategory()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateCategory(CategoryCreateDTO model)
		{
			if (ModelState.IsValid)
			{
				var response = await _categoryService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Kategoria została dodana";
					return RedirectToAction(nameof(IndexCategory));
				}
			}
			TempData["error"] = "Error";
			return View(model);
		}

		//[Authorize(Roles = "admin")]
		public async Task<IActionResult> UpdateCategory(int id)
		{
			var response = await _categoryService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				CategoryDTO model = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));
				return View(_mapper.Map<CategoryUpdateDTO>(model));
			}
			return NotFound();
		}

		//[Authorize(Roles = "admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateCategory(CategoryUpdateDTO model)
		{
			if (ModelState.IsValid)
			{
				TempData["success"] = "Kategoria została zaktualizowana";
				var response = await _categoryService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess)
				{
					return RedirectToAction(nameof(IndexCategory));
				}
			}
			TempData["error"] = "Error";
			return View(model);
		}


		//[Authorize(Roles = "admin")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var response = await _categoryService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				CategoryDTO model = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));
				return View(model);
			}
			return NotFound();
		}

		//[Authorize(Roles = "admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteCategory(CategoryDTO model)
		{
			var response = await _categoryService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				TempData["success"] = "Kategoria została usunięta";
				return RedirectToAction(nameof(IndexCategory));
			}
			TempData["error"] = "Error";
			return View(model);
		}
	}
}
