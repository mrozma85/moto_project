using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class HotsAdsRepository : Repository<Ad>, IHotsAdsRepository
    {
        private readonly MotoDbContext _db;
        public HotsAdsRepository(MotoDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Ad> UpdateAsync(Ad entity)
        {
            entity.UpdatedDate= DateTime.Now;
            _db.Ads.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
