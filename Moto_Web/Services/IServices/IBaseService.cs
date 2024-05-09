using Moto_Web.Models;

namespace Moto_Web.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest); //metoda zapytania do API
    }
}
