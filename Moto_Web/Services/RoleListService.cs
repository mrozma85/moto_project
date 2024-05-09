using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services.IServices;
using Newtonsoft.Json.Linq;

namespace Moto_Web.Services
{
    public class RoleListService : BaseService, IRoleListService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public RoleListService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }

        public Task<T> CreateAsync<T>(ApplicationRole dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = adUrl + "/api/RolesListAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = adUrl + "/api/RolesListAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/RolesListAPI",
                Token = token
            });
        }
        public Task<T> GetAllAsyncName<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/RolesListAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/RolesListAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(ApplicationRole dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = adUrl + "/api/RolesListAPI/" + dto.Id,
                Token = token
            });
        }
    }
}
