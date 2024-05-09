using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IAdNameService
	{
        Task<T> GetAllAsync<T>(string nameCompany, string token);
		Task<T> GetAsync<T>(int id, string token);
	}
}
