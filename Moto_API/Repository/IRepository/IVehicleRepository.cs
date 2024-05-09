using System.Linq.Expressions;
using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface IVehicleRepository :IRepository<Vehicle>
    {
        Task<Vehicle> UpdateAsync(Vehicle entity);
    }
}
