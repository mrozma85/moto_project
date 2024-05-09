using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services.IServices;
using Newtonsoft.Json.Linq;

namespace Moto_Web.Services
{
    public class UserRoleListService : BaseService, IUserRoleListService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public UserRoleListService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }

        public Task<T> DeleteAsync<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = adUrl + "/api/UserRolesListAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/UserRolesListAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(string id, Roles roleName, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = roleName,
                Url = adUrl + "/api/UserRolesListAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(string id, string roleName, /*ApplicationUserRole dto*/ string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                //Data = dto,
                Url = adUrl + "/api/UserRolesListAPI/" + id + "?" + roleName,
                Token = token
            });
        }
    }
}
