using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;
using System.Linq.Expressions;

namespace Moto_API.Repository
{
    public class AdNameRepository : Repository<Ad>, IAdNameRepository
    {
        private readonly MotoDbContext _db;
        public AdNameRepository(MotoDbContext db) : base(db)
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
