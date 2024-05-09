using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company> UpdateAsync(Company entity);
    }
}
