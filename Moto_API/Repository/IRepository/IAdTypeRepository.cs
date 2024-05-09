using Moto_API.Models;
using System.Linq.Expressions;

namespace Moto_API.Repository.IRepository
{
    public interface IAdTypeRepository :IRepository<AdType>
    {
        Task<AdType> UpdateAsync(AdType entity);
    }
}
