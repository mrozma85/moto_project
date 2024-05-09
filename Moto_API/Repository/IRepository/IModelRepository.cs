using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface IModelRepository : IRepository<Model>
    {
        Task<Model> UpdateAsync(Model entity);
    }
}
