using Aspose.Pdf.Operators;
using Moto_Utility;
using Moto_Web.Models;
using Moto_Web.Models.Dto;
using Moto_Web.Services.IServices;
using System.Collections.Generic;
using static System.Net.WebRequestMethods;

namespace Moto_Web.Services
{
    public class AdNameService : BaseService, IAdNameService
	{
        private readonly IHttpClientFactory _clientFactory;
        private string adUrl;
        public AdNameService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            adUrl = configuration.GetValue<string>("ServiceUrls:MotoAPI");
        }

        public Task<T> GetAllAsync<T>(string nameCompany, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
				ApiType = SD.ApiType.GET,
                Url = adUrl + "/api/AdNameAPI?" + "nameCompany=" + nameCompany,
				Token = token
            });
        }

		public Task<T> GetAsync<T>(int id, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = adUrl + "/api/AdNameAPI/" + id,
				Token = token
			});
		}
	}
}
