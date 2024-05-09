using Microsoft.AspNetCore.Http.HttpResults;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class ModelRepository : Repository<Model>, IModelRepository
    {
        private readonly MotoDbContext _db;
        public ModelRepository(MotoDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Model> UpdateAsync(Model entity)
        {
            _db.Models.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
