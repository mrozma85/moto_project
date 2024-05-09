using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services.IServices;

namespace Moto_Web.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public CategoryService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }
        public Task<T> CreateAsync<T>(CategoryCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = adUrl + "/api/CategoryAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = adUrl + "/api/CategoryAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/CategoryAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/CategoryAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(CategoryUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = adUrl + "/api/CategoryAPI/" + dto.Id,
                Token = token
            });
        }
    }
}
