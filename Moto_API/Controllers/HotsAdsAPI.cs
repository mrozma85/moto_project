using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository;
using Moto_API.Repository.IRepository;
using System.Net;

namespace Moto_API.Controllers
{
    [Route("api/HotsAdsAPI")]
    [ApiController]
    public class HotsAdsAPI : ControllerBase
    {
        private readonly IHotsAdsRepository _dbHotsAds;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public HotsAdsAPI(IHotsAdsRepository dbHotsAds, IMapper mapper)
        {
            _dbHotsAds = dbHotsAds;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet(Name = "GetAdsUnauthorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAdsUnauthorize([FromQuery(Name = "filterAdType")] int? adType)
        {
            try
            {
                IEnumerable<Ad> adList = await _dbHotsAds.GetAllAsync(includeProperties: "AdType,Vehicle,Category,ApplicationUser,Image,ImageByte");

                if (adType > 0)
                {
                    adList = await _dbHotsAds.GetAllAsync(u => u.AdTypeId == adType);
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
    }
}
