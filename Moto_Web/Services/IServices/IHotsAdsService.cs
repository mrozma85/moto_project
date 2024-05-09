using Moto_Web.Models.Dto;

namespace Moto_Web.Services.IServices
{
    public interface IHotsAdsService
    {
        Task<T> GetByIdAdType<T>(int id, string token);
    }
}
