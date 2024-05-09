using Aspose.Pdf;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository.IRepository;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace Moto_API.Controllers
{
    [Route("api/AdNameAPI")]
    [ApiController]
    public class AdNameAPIController : ControllerBase
    {
        private readonly IAdNameRepository _dbAd;
        private readonly MotoDbContext _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdNameAPIController(IAdNameRepository dbAd, /*IImageRepository dbImages*/ IMapper mapper, IWebHostEnvironment hostEnvironment, MotoDbContext db)
        {
            _dbAd= dbAd;
            //_dbImages = dbImages;
            _mapper = mapper;
            this._response = new();
            _hostEnvironment = hostEnvironment;
            _db = db;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAds(string nameCompany)
        {
            try
            {
                //IEnumerable<Ad> adList = await _dbAd.GetAllAsync(includeProperties: "AdType,Vehicle,Category,ApplicationUser,Image,ImageByte");
				
                var adName = _db.Ads.Where(a => a.Vehicle.Model.Company.Name == nameCompany).Include(u => u.AdType)
								.Include(u => u.Vehicle).ThenInclude(u => u.Model).ThenInclude(u => u.Company)
								.Include(u => u.Category)
								.Include(u => u.ApplicationUser)
								.Include(u => u.Image)
								.Include(u => u.ImageByte).AsNoTracking();

				_response.Result = adName;
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

        [HttpGet("{id:int}")]
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
                _response.Result = adId;
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
    }
}
