using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly MotoDbContext _db;
        public CategoryRepository(MotoDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Category> UpdateAsync(Category entity)
        {
            entity.UpdatedDate= DateTime.Now;
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
