using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyMauiController : ControllerBase
    {
        private readonly ICompanyRepository _db;

        public CompanyMauiController(ICompanyRepository db)
        {
            _db= db;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            IEnumerable<Company> company = await _db.GetAllAsync();
            return Ok(company);
        }
    }
}
