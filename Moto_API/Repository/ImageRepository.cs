using Microsoft.AspNetCore.Routing.Constraints;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;

namespace Moto_API.Repository
{
    public class ImageRepository : Repository<VehicleImages>, IImageRepository
    {
        private readonly MotoDbContext _db;
        public ImageRepository(MotoDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<VehicleImages> UpdateAsync(VehicleImages entity)
        {
            _db.VehicleImagesDATA.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task PostFileAsync(IFormFile fileData)
        {
            try
            {
                var image = new VehicleImages()
                {
                    Id = 0,
                    FileName= fileData.FileName,
                };

                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    image.FileData = stream.ToArray();
                }

                var result = _db.VehicleImagesDATA.Add(image);
                await _db.SaveChangesAsync();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostMultiFileAsync(List<IFormFile> fileData)
        {
            try
            {
                foreach (IFormFile file in fileData)
                {
                    var fileDetails = new VehicleImages()
                    {
                        Id = 0,
                        FileName = file.FileName, //FileDetails.FileName,
                        AdId = 2075,
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream); //file.FileDetails.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    var result = _db.VehicleImagesDATA.Add(fileDetails);
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
