using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;
using System.Linq.Expressions;

namespace Moto_API.Repository
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        private readonly MotoDbContext _db;
        public VehicleRepository(MotoDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Vehicle> UpdateAsync(Vehicle entity)
        {
            entity.UpdatedDate= DateTime.Now;
            _db.Vehicles.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
