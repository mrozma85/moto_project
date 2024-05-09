using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Data;
using Moto_API.Models;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdMauiUploadFile : ControllerBase
    {
        private readonly MotoDbContext _motodb;

        public AdMauiUploadFile(MotoDbContext motodb)
        {
            _motodb= motodb;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostImage([FromBody] VehicleImages entity)
        {
            VehicleImages model = entity;
            _motodb.VehicleImagesDATA.Add(model);
            await _motodb.SaveChangesAsync();

            return Ok(model);
            //return Ok(new { status = true, message = "Vehicle Addes", entityId = model.Id });    
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PostUpdateImage(int id, [FromBody] VehicleImages entity)
        {
            if (entity == null || id != entity.Id)
            {
                return BadRequest();
            }
            VehicleImages model = entity;
            _motodb.VehicleImagesDATA.Update(model);
            await _motodb.SaveChangesAsync();

            return Ok(model);
        }
    }
}
