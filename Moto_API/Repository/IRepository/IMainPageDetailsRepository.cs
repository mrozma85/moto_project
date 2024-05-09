using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface IMainPageDetailsRepository : IRepository<MainPageDetials>
    {
        Task<MainPageDetials> UpdateAsync(MainPageDetials entity);
    }
}
