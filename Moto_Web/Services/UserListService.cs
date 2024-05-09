using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services.IServices;
using Newtonsoft.Json.Linq;

namespace Moto_Web.Services
{
    public class UserListService : BaseService, IUserListService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public UserListService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }

        public Task<T> DeleteAsync<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = adUrl + "/api/UsersListAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/UsersListAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/UsersListAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(List<ApplicationUser> dto, string roleName, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = adUrl + "/api/UsersListAPI/" + dto[0].Id + "?"+ "roleName=" +roleName,
                Token = token
            });
        }
    }
}
