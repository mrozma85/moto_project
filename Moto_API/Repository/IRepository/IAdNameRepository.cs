using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface IAdNameRepository : IRepository<Ad>
    {
        Task<Ad> UpdateAsync(Ad entity);
    }
}
