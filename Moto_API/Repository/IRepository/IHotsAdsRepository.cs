using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface IHotsAdsRepository : IRepository<Ad>
    {
        Task<Ad> UpdateAsync(Ad entity);
    }
}
