using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;
using System.Linq.Expressions;

namespace Moto_API.Repository
{
    public class AdRepository : Repository<Ad>, IAdRepository
    {
        private readonly MotoDbContext _db;
        public AdRepository(MotoDbContext db) : base(db)
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

        public async Task<Ad> PostAdsImages(List<IFormFile> fileData, Ad entity)
        {
            _db.Ads.Add(entity);
            await _db.SaveChangesAsync();

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
                        AdId = entity.Id,
                        //ImageUrl = baseUrl + "/Images/" + entity.Id + "_" + fileName,
                        ImageUrl = baseUrl + "/motovirtual/" + entity.Id + "_" + fileName,
                    };
                    _db.VehicleImagesURL.Add(img);
                    await _db.SaveChangesAsync();

                    if(file != null)
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
                                    AdId = entity.Id,
                                };
                                _db.VehicleImagesDATA.Add(image);
                                await _db.SaveChangesAsync();
                            }
                        }
                    }

                    //ZAPIS obrazu FILEDATa
                    //List<VehicleImages> photolist = new List<VehicleImages>();
                    //    foreach (IFormFile file in fileData)
                    //    {
                    //        if (formFile.Length > 0)
                    //        {
                    //            using (var memoryStream = new MemoryStream())
                    //            {
                    //                await formFile.CopyToAsync(memoryStream);
                    //                Upload the file if less than 12 MB
                    //                if (memoryStream.Length < 12097152)
                    //                {
                    //                    based on the upload file to create Photo instance.
                    //                    You can also check the database, whether the image exists in the database.  
                    //                    var image = new VehicleImages()
                    //                    {
                    //                        FileData = memoryStream.ToArray(),
                    //                        FileName = formFile.FileName,

                    //                        Vehicle = entity.Vehicle,
                    //                        VehicleId = entity.Vehicle.Id,
                    //                        FileExtension = Path.GetExtension(formFile.FileName),
                    //                        Size = formFile.Length,
                    //                    };
                    //                    add the photo instance to the list.

                    //                    photolist.Add(image);
                    //                    _db.VehicleImagesDATA.Add(image);
                    //                    await _db.SaveChangesAsync();
                    //                }
                    //                else
                    //                {
                    //                    ModelState.AddModelError("File", "The file is too large.");
                    //                }
                    //            }
                    //        }
                    //    }
                    //assign the photos to the Product, using the navigation property.

                    //entity.Vehicle.VehicleImages = photolist;
                }
                    else
                    {
                        //adDTO.ImageUrl = "https://placehold.co/600x900";
                    }
            
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
