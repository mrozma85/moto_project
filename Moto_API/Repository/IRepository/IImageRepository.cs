using Moto_API.Models;

namespace Moto_API.Repository.IRepository
{
    public interface IImageRepository : IRepository<VehicleImages>
    {
        Task<VehicleImages> UpdateAsync(VehicleImages entity);
        public Task PostFileAsync(IFormFile fileData);
        public Task PostMultiFileAsync(List<IFormFile> fileData);
        //Task<Image> DownloadFileById(int fileName, Image entity);
    }
}
