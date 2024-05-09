using Aspose.Pdf;
using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository.IRepository;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace Moto_API.Controllers
{
    [Route("api/AdAPI")]
    [ApiController]
    public class AdAPIController : ControllerBase
    {
        private readonly IAdRepository _dbAd;
        private readonly MotoDbContext _db;
        //private readonly IImageRepository _dbImages;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdAPIController(IAdRepository dbAd, UserManager<ApplicationUser> userManager, /*IImageRepository dbImages*/ IMapper mapper, IWebHostEnvironment hostEnvironment, MotoDbContext db)
        {
            _dbAd= dbAd;
            //_dbImages = dbImages;
            _mapper = mapper;
            this._response = new();
            _hostEnvironment = hostEnvironment;
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAds([FromQuery(Name = "filterCena")] float? price,
                                                            [FromQuery(Name = "filterNazwa")] string? search,
                                                            [FromQuery(Name = "filterDlaUser")] string? idUser,
                                                            [FromQuery(Name = "filterCategory")] int? category,
                                                            [FromQuery(Name = "filterAdType")] int? adType,
                                                            float priceStart,
                                                            float priceEnd,
                                                            int pojemnoscStart,
                                                            int pojemnoscEnd,
                                                            int rokProdukcjiStart,
                                                            int rokProdukcjiEnd,
                                                            string CompanyName
                                                            )
        {
            try
            {
                IEnumerable<Ad> adList = await _dbAd.GetAllAsync(includeProperties: "AdType,Vehicle,Category,ApplicationUser,Image,ImageByte");

				var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var loggedUser = userId.Value;
                idUser = loggedUser;

                //if (!string.IsNullOrEmpty(idUser) && loggedUser == idUser)
                //{
                //    //adList = await _dbAd.GetAllAsync(u => u.ApplicationUserId == idUser);
                //    adList = await _dbAd.GetAllAsync();
                //}
                //else
                //{
                //    BadRequest();
                //}

                if (adType > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.AdTypeId == adType);
                }

                if (category > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Category.Id == category);
                }
                else
                {
                    adList = await _dbAd.GetAllAsync();
                }

				//sort by CompanyName
				if (CompanyName!=null && search == null 
                                      && priceStart == 0
								      && priceEnd == 0
								      && rokProdukcjiEnd == 0
								      && rokProdukcjiStart == 0
								      && pojemnoscStart == 0
								      && pojemnoscEnd == 0)
				{
					var adId = _db.Ads.Include(u => u.AdType)
														.Include(u => u.Vehicle).ThenInclude(u => u.Model).ThenInclude(u => u.Company)
														.Include(u => u.Category)
														.Include(u => u.ApplicationUser)
														.Include(u => u.Image)
														.Include(u => u.ImageByte).AsNoTracking();

					adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search));
				}

				// cena od do bez nazwy
				if (search == null &&  priceStart > 0 && priceStart < priceEnd && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                           && pojemnoscStart ==0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Price < priceEnd );
                }

                // cena od do z nazwa 
                if (search != null && priceStart > 0 && priceEnd > 0
                           && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                           && pojemnoscStart ==0 && pojemnoscEnd == 0)
                {
                    
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Price < priceEnd);
                }

                // cena do z nazwa
                if (search != null && priceStart == 0 
                                   && priceEnd > 0
                                   && pojemnoscStart == 0 
                                   && pojemnoscEnd == 0
                                   && rokProdukcjiStart ==0
                                   && rokProdukcjiEnd ==0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price < priceEnd);

                }

                //cena od z nazwa 
                if (search != null && priceStart > 0 
                                   && priceEnd == 0 
                                   && pojemnoscStart == 0 
                                   && pojemnoscEnd == 0
                                   && rokProdukcjiStart ==0
                                   && rokProdukcjiEnd ==0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price > priceStart);

                }

                //nazwa
                if (search != null && priceStart == 0 
                                   && priceEnd == 0 
                                   && rokProdukcjiEnd == 0 
                                   && rokProdukcjiStart == 0
                                   && pojemnoscStart == 0
                                   && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search));
                }

                //cena od bez nazwy
                if (search == null && priceStart > 0 
                                   && priceEnd == 0 
                                   && pojemnoscStart == 0 
                                   && pojemnoscEnd == 0
                                   && rokProdukcjiEnd == 0
                                   && rokProdukcjiStart == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price > priceStart);
                }

                //cena do bez nazwy
                if (search == null && priceStart == 0 
                                   && priceEnd < 0
                                   && pojemnoscStart == 0
                                   && pojemnoscEnd == 0
                                   && rokProdukcjiEnd == 0
                                   && rokProdukcjiStart == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price < priceStart);
                }

                //nazwa i cena od i pojemnosc do
                if (search != null && priceStart > 0 
                                   && pojemnoscStart > 0 
                                   && priceEnd == 0 
                                   && pojemnoscEnd == 0 
                                   && rokProdukcjiStart == 0 
                                   && rokProdukcjiEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Engine >= pojemnoscStart);

                }

                //nazwa i cena do i pojemnosc od
                if (search != null && priceEnd > 0 
                                   && pojemnoscStart > 0
                                   && pojemnoscEnd == 0
                                   && priceStart == 0
                                   && rokProdukcjiStart == 0
                                   && rokProdukcjiEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Engine >= pojemnoscStart);

                }

                //nazwa i cena od i pojemnosc do
                if (search != null && priceStart > 0 
                                   && pojemnoscEnd > 0
                                   && pojemnoscStart == 0
                                   && priceEnd == 0
                                   && rokProdukcjiStart == 0
                                   && rokProdukcjiEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //nazwa i cena do i pojemnosc od
                if (search != null && priceEnd > 0 
                                   && pojemnoscEnd > 0 
                                   && priceStart == 0
                                   && pojemnoscStart == 0
                                   && rokProdukcjiStart == 0
                                   && rokProdukcjiEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //bez nazwa i cena od i pojemnosc od
                if (search == null && priceStart > 0 
                                   && pojemnoscEnd > 0
                                   && pojemnoscStart == 0
                                   && priceEnd == 0
                                   && rokProdukcjiStart == 0
                                   && rokProdukcjiEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //bez nazwa i cena od i pojemnosc do
                if (search == null && priceStart > 0 
                                   && pojemnoscStart > 0
                                   && pojemnoscEnd == 0
                                   && priceEnd == 0
                                   && rokProdukcjiStart == 0
                                   && rokProdukcjiEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Engine >= pojemnoscStart);

                }

                //bez nazwa i cena do i pojemnosc do
                if (search == null && priceEnd > 0 
                                   && pojemnoscStart > 0 
                                   && priceStart == 0
                                   && pojemnoscEnd == 0
                                   && rokProdukcjiStart == 0
                                   && rokProdukcjiEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                //bez nazwa i cena do i pojemnosc od
                if (search == null && priceEnd > 0 
                                   && pojemnoscEnd > 0
                                   && pojemnoscStart == 0
                                   && priceStart == 0
                                   && rokProdukcjiStart == 0
                                   && rokProdukcjiEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //rok od bez nazwy
                if (search == null && rokProdukcjiStart > 0 
                                   && rokProdukcjiEnd == 0
                                   && pojemnoscStart == 0
                                   && priceStart == 0
                                   && priceEnd == 0
                                   && pojemnoscEnd == 0
                                   && pojemnoscStart == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Year >= rokProdukcjiStart);
                }

                //rok do bez nazwy
                if (search == null && rokProdukcjiEnd > 0 
                                   && rokProdukcjiStart == 0
                                   && pojemnoscStart == 0
                                   && priceStart == 0
                                   && priceEnd == 0
                                   && pojemnoscEnd == 0
                                   && pojemnoscStart == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Year <= rokProdukcjiEnd);
                }

                // rok do do bez nazwy
                if (search == null && rokProdukcjiStart > 0 
                                   && rokProdukcjiStart < rokProdukcjiEnd
                                   && priceStart == 0
                                   && priceEnd == 0
                                   && pojemnoscEnd == 0
                                   && pojemnoscStart == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                //rok od z nazwa
                if (search != null && rokProdukcjiStart > 0 
                                   && rokProdukcjiEnd == 0
                                   && priceStart == 0
                                   && priceEnd == 0
                                   && pojemnoscEnd == 0
                                   && pojemnoscStart == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Year >= rokProdukcjiStart);
                }

                //rok do z nazwa
                if (search != null && rokProdukcjiEnd > 0 
                                   && rokProdukcjiStart == 0
                                   && priceStart == 0
                                   && priceEnd == 0
                                   && pojemnoscEnd == 0
                                   && pojemnoscStart == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Year >= rokProdukcjiEnd);
                }

                // rok od do z nazwa
                if (search != null &&  rokProdukcjiStart > 0 
                                   && rokProdukcjiStart < rokProdukcjiEnd
                                   && priceStart == 0
                                   && priceEnd == 0
                                   && pojemnoscEnd == 0
                                   && pojemnoscStart == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                // nazwa cena od + poj od + rok od
                if (search != null && priceStart > 0 
                                   && priceEnd == 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Engine >= pojemnoscStart);
                }

                // nazwa cena od + poj od + rok do
                if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                // nazwa cena od + poj od + rok do/do
                if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                // bez nazwa cena od + poj od + rok od
                if (search == null && priceStart > 0
                                   && priceEnd == 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Engine >= pojemnoscStart);
                }

                // bez nazwa cena od + poj od + rok do
                if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                // bez nazwa cena od + poj od + rok do/do
                if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                //pojemnosc silnka od bez nazwy
                if (search == null
                    && priceStart == 0 && priceEnd == 0
                    && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                    && priceStart == 0 && priceEnd == 0
                    && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Engine > pojemnoscStart);
                }

                //pojemnosc silnka do bez nazwy
                if (search == null
                    && priceStart == 0 && priceEnd == 0
                    && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                    && priceStart == 0 && priceEnd == 0
                    && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Engine < pojemnoscEnd);
                }

                // bez nazwa cena do + poj od + rok od
                if (search == null && priceStart == 0
                                   && priceEnd > 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Engine >= pojemnoscStart);
                }

                // bez nazwa cena do + poj od + rok do
                if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                // bez nazwa cena do + poj od + rok do/do
                if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                // nazwa cena do + poj od + rok od
                if (search != null && priceStart == 0
                                   && priceEnd > 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Engine >= pojemnoscStart);
                }

                // nazwa cena do + poj od + rok do
                if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                // nazwa cena do + poj od + rok do/do
                if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine >= pojemnoscStart);
                }

                // bez nazwa cena do + poj do + rok do
                //
                if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine <= pojemnoscEnd);
                }

                // nazwa cena do + poj do + rok od
                if (search != null && priceStart == 0
                                   && priceEnd > 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                // nazwa cena do + poj do + rok do
                if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine <= pojemnoscEnd);
                }

                // nazwa cena do + poj do + rok do/do
                if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine <= pojemnoscEnd);
                }

                // bez nazwa cena do + poj do + rok od
                if (search == null && priceStart == 0
                                   && priceEnd > 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                // bez nazwa cena do + poj do + rok do
                if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine <= pojemnoscEnd);
                }

                // bez nazwa cena do + poj do + rok do/do
                if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Engine <= pojemnoscEnd);
                }

                // bez nazwa cena od + rok od
                if (search == null && priceStart > 0
                                   && priceEnd == 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Year >= rokProdukcjiStart);
                }

                // bez nazwa cena od + rok do
                if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                // bez nazwa cena od rok do/do
                    if (search == null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                // nazwa cena od + rok od
                if (search != null && priceStart > 0
                                   && priceEnd == 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Year >= rokProdukcjiStart);
                }

                // nazwa cena od + rok do
                if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                // nazwa cena od rok do/do
                if (search != null && priceStart > 0 && priceEnd == 0
                           && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                           && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }
                // bez nazwa cena do + rok od
                if (search == null && priceStart == 0
                                   && priceEnd > 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart);
                }

                // bez nazwa cena do + rok do
                if (search == null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                // bez nazwa cena do rok do/od
                if (search == null && priceStart == 0 && priceEnd > 0
                           && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                           && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                // nazwa cena do rok do/od
                if (search != null && priceStart == 0 && priceEnd > 0
                           && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                           && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }


                // nazwa cena do + rok od
                if (search != null && priceStart == 0
                                   && priceEnd > 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                                   && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart);
                }

                // nazwa cena do + rok do
                if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                // nazwa cena do rok do/do
                if (search != null && priceStart == 0 && priceEnd > 0
                           && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                           && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                //bez nazwa + cena od / pojemnosc do + rok od/do
                if (search == null && priceStart > 0 && priceEnd == 0
                                   && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                                   && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Engine <= pojemnoscEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                //bez nazwa cena od/do + rok od/do
                if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                //nazwa cena od/do + rok od/do
                if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                //nazwa cena od/do + rok od
                if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart);
                }

                //nazwa cena od/do + rok do
                if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Price >= priceStart && u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                //bez nazwa cena od/do + rok od
                if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Price <= priceEnd && u.Vehicle.Year >= rokProdukcjiStart);
                }

                //bez nazwa cena od/do + rok do
                if (search == null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart == 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Price <= priceEnd && u.Vehicle.Year <= rokProdukcjiEnd);
                }

                //bez nazwa poj od do
                if (search == null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Engine >= pojemnoscStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //nazwa poj od
                if (search != null
                    && priceStart == 0 && priceEnd == 0
                    && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                    && priceStart == 0 && priceEnd == 0
                    && pojemnoscStart > 0 && pojemnoscEnd == 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine > pojemnoscStart);
                }

                //nazwa poj do
                if (search != null
                    && priceStart == 0 && priceEnd == 0
                    && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                    && priceStart == 0 && priceEnd == 0
                    && pojemnoscStart == 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine < pojemnoscEnd);
                }

                //nazwa poj od do
                if (search != null && priceStart == 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine >= pojemnoscStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //nazwa cena od poj od do
                if (search != null && priceStart > 0 && priceEnd == 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine >= pojemnoscStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //nazwa cena do poj od do
                if (search != null && priceStart == 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price <= priceEnd && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine >= pojemnoscStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //nazwa cena od do poj od do
                if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Price >= priceStart && u.Vehicle.Price <= priceEnd && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine >= pojemnoscStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //nazwa cena od do poj od do rok od
                if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd == 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Price >= priceStart && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine >= pojemnoscStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //nazwa cena od do poj od do rok do
                if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart == 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Price >= priceStart && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine >= pojemnoscStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                //nazwa cena od do poj od do rok od do 
                if (search != null && priceStart > 0 && priceEnd > 0
                               && rokProdukcjiStart > 0 && rokProdukcjiEnd > 0
                               && pojemnoscStart > 0 && pojemnoscEnd > 0)
                {
                    adList = await _dbAd.GetAllAsync(u => u.Vehicle.Year >= rokProdukcjiStart && u.Vehicle.Year <= rokProdukcjiEnd && u.Vehicle.Price >= priceStart && u.Vehicle.Title.ToLower().Contains(search) && u.Vehicle.Engine >= pojemnoscStart && u.Vehicle.Engine <= pojemnoscEnd);
                }

                _response.Result = _mapper.Map<List<AdDTO>>(adList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetAd")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAd(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var ad = await _dbAd.GetAsync(u => u.Id == id, includeProperties: "AdType,Vehicle,Category,ApplicationUser,Image,ImageByte");

                var adId = _db.Ads.Where(a => a.Id ==id).Include(u => u.AdType)
                                                        .Include(u => u.Vehicle).ThenInclude(u => u.Model).ThenInclude(u => u.Company)
                                                        .Include(u => u.Category)
                                                        .Include(u => u.ApplicationUser)
                                                        .Include(u => u.Image)
                                                        .Include(u => u.ImageByte).AsNoTracking();

                if (ad == null)
                {
                    return NotFound();
                }

                //_response.Result = adId;
                _response.Result = _mapper.Map<AdDTO>(ad);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

		[HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        

        public async Task<ActionResult<APIResponse>> CreateAd(/*[FromForm] List<IFormFile> fileDetails, */[FromBody] AdDTO adDTO)
        {
            try
            {
                //if(fileDetails == null)
                //{
                //    return BadRequest();
                //}

                if (adDTO == null)
                {
                    return BadRequest(adDTO);
                }

                if (adDTO.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Ad model = _mapper.Map<Ad>(adDTO);
                await _dbAd.CreateAsync(model);
                //await _dbAd.PostAdsImages(fileDetails, model);
                _response.Result = _mapper.Map<AdDTO>(adDTO);
                _response.StatusCode = HttpStatusCode.Created;
                //return CreatedAtRoute("GetAd", new { id = model.Id }, _response);
                return Ok(new { status = true, message = "Ogloszenie dodane", adId = model.Id });

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()
                };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteAd")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteAd(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var ad = await _dbAd.GetAsync(u => u.Id == id);
                if (ad == null)
                {
                    return NotFound();
                }
                await _dbAd.RemoveAsync(ad);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpPut("{id:int}", Name = "UpdateAd")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateAd(int id, [FromBody] AdDTO adDTO)
        {
            try
            {
                if (adDTO == null || id != adDTO.Id)
                {
                    return BadRequest();
                }
                //if (await _dbAd.GetAsync(u => u.Id == adDTO.Id) == null)
                //{
                //    ModelState.AddModelError("CustomError", "Ad ID is invalid");
                //    return BadRequest(ModelState);
                //}

                Ad model = _mapper.Map<Ad>(adDTO);

                await _dbAd.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("ExportExcel")]
        public ActionResult ExportExcel()
        {
            var getData = GetData();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet(getData, "Ads Records");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdsRecords.xlsx");
                }
            }
        }

        [NonAction]
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "AdsRecordsData";
            dt.Columns.Add("Nr ogłoszenia", typeof(int));
            dt.Columns.Add("Data utworzenia", typeof(DateTime));
            dt.Columns.Add("Data aktualizacji", typeof(DateTime));
            dt.Columns.Add("Typ ogłoszenia", typeof(string));
            dt.Columns.Add("Nazwa kategorii", typeof(string));
            dt.Columns.Add("Użytkownik id", typeof(string));
            dt.Columns.Add("Nazwa użytkownika", typeof(string));


            var list = _db.Ads.Include(c => c.Category).Include(a => a.AdType).Include(u => u.ApplicationUser).ToList();

            var adlist = _dbAd.GetAllAsync(includeProperties: "AdType,Vehicle,Category,ApplicationUser,Image,ImageByte");

            if (list.Count > 0)
            {
                list.ForEach(item =>
                {
                    dt.Rows.Add(item.Id, item.CreatedDate, item.UpdatedDate, item.AdType.Name, item.Category.Name, item.ApplicationUserId, item.ApplicationUser.UserName);
                });
            }
            return dt;
        }

        [HttpGet("ExportVehicleExcel")]
        public ActionResult ExportVehicleExcel()
        {
            var getData = GetDataVehicle();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet(getData, "Lista pojazdów");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdsRecords.xlsx");
                }
            }
        }

        [HttpGet("ExportVehiclePDF")]
        public ActionResult ExportVehiclePDF()
        {
            var getData = GetDataVehicle();
            var document = new Document
            {
                PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40) }
            };
            var pdfpage = document.Pages.Add();
            Table table = new Table
            {
                ColumnWidths = "5% 15% 15% 15% 15% 25% 15%",
                DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                Border = new BorderInfo(BorderSide.All, .2f, Color.Black),
            };
            table.ImportDataTable(getData, true, 0, 0);
            document.Pages[1].Paragraphs.Add(table);

            using (var streamout = new MemoryStream())
            {
                document.Save(streamout);
                return new FileContentResult(streamout.ToArray(), "application/pdf")
                {
                    FileDownloadName="Lista pojazdów.pdf"
                };
            }
        }

        [NonAction]
        private DataTable GetDataVehicle()
        {
            DataTable dt = new DataTable();
            dt.TableName = "VehicleRecords";
            dt.Columns.Add("Nr ogłoszenia", typeof(int));
            dt.Columns.Add("Data utworzenia", typeof(DateTime));
            dt.Columns.Add("Typ ogłoszenia", typeof(string));
            dt.Columns.Add("Nazwa kategorii", typeof(string));
            dt.Columns.Add("Marka", typeof(string));
            dt.Columns.Add("Model", typeof(string));
            dt.Columns.Add("Rok produkcji", typeof(string));
            dt.Columns.Add("Cena", typeof(double));
            dt.Columns.Add("Nazwa użytkownika", typeof(string));


            var list = _db.Ads.Include(c => c.Category).Include(a => a.AdType).Include(u => u.ApplicationUser)
                        .Include(u => u.Vehicle).ThenInclude(u => u.Model).ThenInclude(u => u.Company).ToList();

            if (list.Count > 0)
            {
                list.ForEach(item =>
                {
                    dt.Rows.Add(item.Id, item.CreatedDate, item.AdType.Name, item.Category.Name, item.Vehicle.Model.Company.Name, 
                                item.Vehicle.Model.Name, item.Vehicle.Year, item.Vehicle.Price, item.ApplicationUser.UserName);
                });
            }
            return dt;
        }

        [HttpGet("ExportPDF")]
        public ActionResult ExportPDF()
        {
            var getData = GetDataVehicle();
            var document = new Document
            {
                PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40), IsLandscape=true }
            };
            var pdfpage = document.Pages.Add();
            Table table = new Table
            {
                ColumnWidths = "10% 10% 10% 10% 10% 10% 10% 10% 20%",
                DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                Border = new BorderInfo(BorderSide.All, .2f, Color.Black),
            };
            table.ImportDataTable(getData, true, 0, 0);
            document.Pages[1].Paragraphs.Add(table);

            using (var streamout = new MemoryStream())
            {
                document.Save(streamout);
                return new FileContentResult(streamout.ToArray(), "application/pdf")
                {
                    FileDownloadName="AdsRecords.pdf"
                };
            }
        }


        [NonAction]
        private DataTable GetDataUserAds()
        {
            DataTable dt = new DataTable();
            dt.TableName = "UserAds";
            dt.Columns.Add("Nazwa użytkownika", typeof(string));
            dt.Columns.Add("Rola przypisana", typeof(string));

            var users = _userManager.Users.Include(u => u.UserRoles)
                                              .ThenInclude(ur => ur.Role).ToList();

            foreach (var item in users)
            {
                var name = item.UserName;

                foreach (var userRole in item.UserRoles)
                {
                    dt.Rows.Add(name, userRole.Role.Name);                    
                }     
            }
            return dt;
        }

        [HttpGet("GetDataUserAdsEXCEL")]
        public ActionResult GetDataUserAdsEXCEL()
        {
            var getData = GetDataUserAds();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet(getData, "Lista użytkowników");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AdsRecords.xlsx");
                }
            }
        }

        [HttpGet("GetDataUserAdsPDF")]
        public ActionResult GetDataUserAdsPDF()
        {
            var getData = GetDataUserAds();
            var document = new Document
            {
                PageInfo = new PageInfo { Margin = new MarginInfo(28, 28, 28, 40) }
            };
            var pdfpage = document.Pages.Add();
            Table table = new Table
            {
                ColumnWidths = "30% 30%",
                DefaultCellPadding = new MarginInfo(10, 5, 10, 5),
                Border = new BorderInfo(BorderSide.All, .2f, Color.Black),
            };
            table.ImportDataTable(getData, true, 0, 0);
            document.Pages[1].Paragraphs.Add(table);

            using (var streamout = new MemoryStream())
            {
                document.Save(streamout);
                return new FileContentResult(streamout.ToArray(), "application/pdf")
                {
                    FileDownloadName="Lista użytkowników.pdf"
                };
            }
        }

    }
}