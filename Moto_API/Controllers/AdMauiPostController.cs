using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Repository.IRepository;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdMauiPostController : ControllerBase
    {
        private readonly IAdRepository _db;
        private readonly MotoDbContext _motodb;
        private readonly IMapper _mapper;

        public AdMauiPostController(IAdRepository db, MotoDbContext motodb, IMapper mapper)
        {
            _db= db;
            _motodb= motodb;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAd([FromBody] AdDTO entity/*, HttpRequest httpRequest*/)
        {
            Ad model = _mapper.Map<Ad>(entity);
            _motodb.Ads.Add(model);
            await _motodb.SaveChangesAsync();

            return Ok(new { status = true, message = "Ad Added", entityId = model.Id, vehicleId = model.Vehicle.Id});
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> PostAdImage(HttpRequest httpRequest)
        //{
        //    if (!httpRequest.HasFormContentType)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        var formCollection = await httpRequest.ReadFormAsync();

        //        var iFormFile = formCollection.Files["fileContent"];

        //        if (iFormFile is null || iFormFile.Length == 0)
        //        {
        //            return BadRequest();
        //        }

        //        using var stream = iFormFile.OpenReadStream();

        //        var localFilePath = Path.Combine("Images", iFormFile.FileName);

        //        using var localFileStream = File.OpenWrite(localFilePath);

        //        await stream.CopyToAsync(localFileStream);

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        //app.Logger.LogError(ex.Message);
        //        return BadRequest();
        //    }
        //}
    }
}
