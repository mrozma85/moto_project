using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Models.VM;
using Moto_Web.Services.IServices;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace Moto_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHotsAdsService _hotAdsService;
        private readonly IMainPageDetailsService _mainPageDetailsService;
        private readonly IAdNameService _adNameService;
        private readonly IAdService _adService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IAdService adService, IAdNameService adNameService
                                                      , IHotsAdsService hotAdsService, IMainPageDetailsService mainPageDetailsService)
        {
            _logger = logger;
            _mapper = mapper;
            _adService = adService;
            _hotAdsService = hotAdsService;
            _adNameService = adNameService;
            _mainPageDetailsService = mainPageDetailsService;
		}

        public async Task<IActionResult> Index(int idAdType, string search, float priceStar, float priceEnd, 
                                               int rokProdukcjiStart, int rokProdukcjiEnd,
                                               int pojemnoscStart,
                                               int pojemnoscEnd, 
                                               string searchLocation)
        {
            HomeVM home = new();

            List<MainPageDetails> main1 = new();
            var mainResponse = await _mainPageDetailsService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (mainResponse != null && mainResponse.IsSuccess)
            {
                main1 = JsonConvert.DeserializeObject<List<MainPageDetails>>(Convert.ToString(mainResponse.Result));
            }
            home.Main = main1;



            List<AdDTO> list = new();
            idAdType = 2; // id = platne

            if(search == null && priceStar == 0 && priceEnd == 0 
                              && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                              && pojemnoscStart == 0 && pojemnoscEnd == 0
                              && searchLocation == null)
            {
                var response = await _hotAdsService.GetByIdAdType<APIResponse>(idAdType, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<AdDTO>>(Convert.ToString(response.Result));
                }
            }
            else
            {
                var response = await _adService.GetByName<APIResponse>(search, priceStar, priceEnd, rokProdukcjiStart, 
                                                                       rokProdukcjiEnd, pojemnoscStart, pojemnoscEnd,
                                                                       searchLocation, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<AdDTO>>(Convert.ToString(response.Result));
                }
            }
            home.Ad = list;
            return View(home);
        }

        [Authorize]
        public async Task<IActionResult> VehiclesList(string search, float priceStar, float priceEnd,
                                               int rokProdukcjiStart, int rokProdukcjiEnd,
                                               int pojemnoscStart,
                                               int pojemnoscEnd,
                                               string searchLocation)
        {
            HomeVM home = new();
            List<AdDTO> list = new();

                var response = await _adService.GetByName<APIResponse>(search, priceStar, priceEnd, rokProdukcjiStart,
                                                                       rokProdukcjiEnd, pojemnoscStart, pojemnoscEnd,
                                                                       searchLocation, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<AdDTO>>(Convert.ToString(response.Result));
                }
            home.Ad = list;
            return View(home);
        }

        [Authorize]
        public async Task<IActionResult> DetailsVehicle(int id)
        {
            HomeVM home = new();
            DetailsVehicleVM detailsVehicleVW= new();
			
			var response = await _adNameService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
				List<AdDTO> model = JsonConvert.DeserializeObject<List<AdDTO>>(Convert.ToString(response.Result));
				home.Ad = model;
			}

            var name = home.Ad[0].Vehicle.Model.Company.Name;

			var response2 = await _adNameService.GetAllAsync<APIResponse>(name, HttpContext.Session.GetString(SD.SessionToken));
			if (response2 != null && response2.IsSuccess)
			{
				List<AdDTO> modelbyName = JsonConvert.DeserializeObject<List<AdDTO>>(Convert.ToString(response2.Result));
                
                List<AdDTO> listNoDuplicate = new();
				var items = modelbyName.ExceptBy(home.Ad.Select(x => x.Id), x => x.Id);
				var itemsTwo = items.OrderBy(x => Guid.NewGuid()).Take(2);

				foreach (var item in itemsTwo)
                {
                    listNoDuplicate.Add(item);
                }
                home.AdName = listNoDuplicate;

				return View(home);
			}

			return NotFound();

		}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}