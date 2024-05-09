using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelMauiController : ControllerBase
    {
        private readonly IModelRepository _db;
        private readonly MotoDbContext _motodb;

        public ModelMauiController(IModelRepository db, MotoDbContext motodb)
        {
            _db= db;
            _motodb= motodb;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Model>>> GetModels()
        {
            //IEnumerable<Model> model = await _db.GetAllAsync();

            var model = _motodb.Models.Include(u => u.Company);
            return Ok(model);

        }
    }
}
