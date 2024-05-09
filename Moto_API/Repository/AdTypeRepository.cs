using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class AdTypeRepository : Repository<AdType>, IAdTypeRepository
    {
        private readonly MotoDbContext _db;
        public AdTypeRepository(MotoDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<AdType> UpdateAsync(AdType entity)
        {
            //entity.UpdatedDate= DateTime.Now;
            _db.AdTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
