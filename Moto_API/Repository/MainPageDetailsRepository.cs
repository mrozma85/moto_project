using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class MainPageDetailsRepository : Repository<MainPageDetials>, IMainPageDetailsRepository
    {
        private readonly MotoDbContext _db;

        public MainPageDetailsRepository(MotoDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<MainPageDetials> UpdateAsync(MainPageDetials entity)
        {
            _db.MainPageDetail.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
