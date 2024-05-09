using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services.IServices;

namespace Moto_Web.Services
{
    public class ModelService : BaseService, IModelService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public ModelService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }

        public Task<T> CreateAsync<T>(Model dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = adUrl + "/api/ModelAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = adUrl + "/api/ModelAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/ModelAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/ModelAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(Model dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = adUrl + "/api/ModelAPI/" + dto.Id,
                Token = token
            });
        }
    }
}
