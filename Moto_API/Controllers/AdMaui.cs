using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using Moto_API.Models.Dto.Category;
using Moto_API.Repository.IRepository;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdMaui : ControllerBase
    {
        private readonly IAdRepository _db;
        private readonly ICategoryRepository _categoryDb;
        private readonly MotoDbContext _motodb;
        private readonly IMapper _mapper;

        public AdMaui(IAdRepository db, MotoDbContext motodb, IMapper mapper)
        {
            _db= db;
            _motodb= motodb;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AdDTO>>> GetAds([FromQuery(Name = "filterNazwa")] string? search)
        {
            IEnumerable<Ad> adList = await _db.GetAllAsync(includeProperties: "AdType,Vehicle,Category,ApplicationUser,Image,ImageByte");

            if (search != null)
            {
                adList = await _db.GetAllAsync(u => u.Vehicle.Title.ToLower().Contains(search));
            }
            return Ok(adList);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<AdDTO>> GetAd(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var ad = await _db.GetAsync(u => u.Id == id, includeProperties: "AdType,Vehicle,Category,ApplicationUser,Image,ImageByte");

            var adId = _motodb.Ads.Where(a => a.Id ==id).Include(u => u.AdType)
                                                        .Include(u => u.Vehicle).ThenInclude(u => u.Model).ThenInclude(u => u.Company)
                                                        .Include(u => u.Category)
                                                        .Include(u => u.ApplicationUser)
                                                        .Include(u => u.Image)
                                                        .Include(u => u.ImageByte).AsNoTracking();

            if (ad == null)
            {
                return NotFound();
            }
            return Ok(adId);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAd(List<IFormFile> fileData, [FromBody] AdDTO entity)
        {
            Ad model = _mapper.Map<Ad>(entity);
            _motodb.Ads.Add(model);
            await _motodb.SaveChangesAsync();

            foreach (IFormFile file in fileData)
                if (fileData != null)
                {
                    string fileName = file.FileName;
                    string filePath = @"Images\" + entity.Id + "_" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    //var baseUrl = "https://localhost:7001";

                    var baseUrl = "localhost:8020";

                    Image img = new Image()
                    {
                        Id = 0,
                        AdId = model.Id,
                        //ImageUrl = baseUrl + "/Images/" + entity.Id + "_" + fileName,
                        ImageUrl = baseUrl + "/motovirtual/" + entity.Id + "_" + fileName,
                    };
                    _motodb.VehicleImagesURL.Add(img);
                    await _motodb.SaveChangesAsync();

                    if (file != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            //Upload the file if less than 12 MB
                            if (memoryStream.Length < 12097152)
                            {
                                var image = new VehicleImages()
                                {
                                    FileName = file.FileName,
                                    FileData = memoryStream.ToArray(),
                                    AdId = model.Id,
                                };
                                _motodb.VehicleImagesDATA.Add(image);
                                await _motodb.SaveChangesAsync();
                            }
                        }
                    }
                }
                else
                {
                    //adDTO.ImageUrl = "https://placehold.co/600x900";
                }

            await _motodb.SaveChangesAsync();

            //return Ok(model);
            return Ok(new { status = true, message = "Ad Added", entityId = model.Id });
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> PostAd([FromBody] AdDTO entity)
        //{
        //    Ad model = _mapper.Map<Ad>(entity);
        //    _motodb.Ads.Add(model);
        //    await _motodb.SaveChangesAsync();

        //    return Ok(model);
        //}

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<AdDTO>> DeleteAd(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var ad = await _db.GetAsync(u => u.Id == id);
            if (ad == null)
            {
                return NotFound();
            }
            await _db.RemoveAsync(ad);
            return Ok();
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<AdDTO>> UpdateAd(int id, [FromBody] AdDTO adDTO)
        {
            if (adDTO == null || id != adDTO.Id)
            {
                return BadRequest();
            }

            Ad model = _mapper.Map<Ad>(adDTO);

            await _db.UpdateAsync(model);

            return Ok();
        }
    }
}
