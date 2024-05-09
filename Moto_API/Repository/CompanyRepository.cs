using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly MotoDbContext _db;
        public CompanyRepository(MotoDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Company> UpdateAsync(Company entity)
        {
            _db.Companies.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
