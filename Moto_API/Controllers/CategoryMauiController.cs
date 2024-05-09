using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryMauiController : ControllerBase
    {
        private readonly ICategoryRepository _db;

        public CategoryMauiController(ICategoryRepository db)
        {
            _db= db;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            IEnumerable<Category> category = await _db.GetAllAsync();
            return Ok(category);
        }
    }
}
