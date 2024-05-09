using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface IAdRepository :IRepository<Ad>
    {
        Task<Ad> UpdateAsync(Ad entity);
        Task<Ad> PostAdsImages(List<IFormFile> fileData, Ad entity);
    }
}
