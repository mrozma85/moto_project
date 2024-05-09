using Moto_Web.Models;
using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IAdService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> GetByName<T>(string search, 
                             float priceStart, float priceEnd, 
                             int rokProdukcjiStart, int rokProdukcjiEnd,
                             int pojemnoscStart,
                             int pojemnoscEnd,
                             string searchLocation, 
                             string token);
        Task<T> GetByRangePrice<T>(float priceStart, float priceEnd, string token);
        Task<T> GetByPojemnosc<T>(int pojemnoscStart, int pojemnoscEnd, string token);
        Task<T> GetByIdAdType<T>(int id, string token);
        Task<T> CreateAsync<T>(AdCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(AdUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);

        Task<T> CreateAsyncNew<T>(AdCreateDTO dto, string token);
        Task<T> GetAsyncByUser<T>(string user, string token);
    }
}
