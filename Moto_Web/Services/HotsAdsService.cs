using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services.IServices;

namespace Moto_Web.Services
{
    public class HotsAdsService : BaseService, IHotsAdsService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public HotsAdsService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }

        public Task<T> GetByIdAdType<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/HotsAdsAPI/?filterAdType=" + id,
                Token = token
            });
        }

        
    }
}
