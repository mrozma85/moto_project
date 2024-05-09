using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Services.IServices;

namespace Moto_Web.Services
{
    public class ImageService : BaseService, IImageService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public ImageService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }

        public Task<T> CreateAsync<T>(VehicleImages dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = adUrl + "/api/AdMauiUploadFile",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = adUrl + "/api/ImagesAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/ImagesAPI",
                Token = token
            });
        }

        public Task<T> GetAllByAdId<T>(int id, VehicleImages dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = dto,
                Url = adUrl + "/api/AdMauiImages/" + id,
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/AdMauiImages/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(int id,VehicleImages dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = adUrl + "/api/AdMauiUploadFile/" + id,
                Token = token
            });
        }
    }
}
