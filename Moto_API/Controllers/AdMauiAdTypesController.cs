using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdMauiAdTypesController : ControllerBase
    {
        private readonly IAdTypeRepository _db;

        public AdMauiAdTypesController(IAdTypeRepository db)
        {
            _db= db;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AdType>>> GetAdTypes()
        {
            IEnumerable<AdType> adType = await _db.GetAllAsync();
            return Ok(adType);
        }
    }
}
