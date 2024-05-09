using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moto_API.Data;
using Moto_API.Repository.IRepository;
using Moto_API.Models;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdMauiImages : ControllerBase
    {
        private readonly IImageRepository _db;
        private readonly MotoDbContext _motodb;

        public AdMauiImages(IImageRepository db, MotoDbContext motodb)
        {
            _db= db;
            _motodb= motodb;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<VehicleImages>>> GetAds()
        {
            IEnumerable<VehicleImages> images = await _db.GetAllAsync();
            return Ok(images);
        }

        [HttpGet("{id:int}")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<VehicleImages>>> GetAdImages(int id)
        {
            var images = await _db.GetAllAsync(u => u.AdId == id);
            if (images == null)
            {
                return NotFound();
            }
            return Ok(images);
        }

        [HttpDelete("{id:int}")]
        //[Authorize]
        public async Task<ActionResult<VehicleImages>> DeleteImages(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var ad = await _db.GetAsync(u => u.AdId == id);
            if (ad == null)
            {
                return NotFound();
            }
            await _db.RemoveAsync(ad);
            return Ok();
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult<IEnumerable<VehicleImages>>> PostAdImages(List<IFormFile> fileData, int id)
        //{
        //    foreach (IFormFile file in fileData)
        //        if (fileData != null)
        //        {
        //            string fileName = file.FileName;
        //            string filePath = @"Images\" + "entity.Id" + "_" + fileName;
        //            var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        //            using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }

        //            var baseUrl = "https://localhost:7001";

        //            var baseUrl = "localhost:8020";

        //            Image img = new Image()
        //            {
        //                Id = 0,
        //                AdId = id,
        //                ImageUrl = baseUrl + "/Images/" + entity.Id + "_" + fileName,
        //                ImageUrl = baseUrl + "/motovirtual/" + "entity.Id" + "_" + fileName,
        //            };
        //            _motodb.VehicleImagesURL.Add(img);
        //            await _motodb.SaveChangesAsync();

        //            if (file != null)
        //            {
        //                using (var memoryStream = new MemoryStream())
        //                {
        //                    await file.CopyToAsync(memoryStream);
        //                    Upload the file if less than 12 MB
        //                    if (memoryStream.Length < 12097152)
        //                    {
        //                        var image = new VehicleImages()
        //                        {
        //                            FileName = file.FileName,
        //                            FileData = memoryStream.ToArray(),
        //                            AdId = id,
        //                        };
        //                        _motodb.VehicleImagesDATA.Add(image);
        //                        await _motodb.SaveChangesAsync();
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            adDTO.ImageUrl = "https://placehold.co/600x900";
        //        }

        //    await _motodb.SaveChangesAsync();

        //    return Ok();
        //}
    }
}
