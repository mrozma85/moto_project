using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_API.Models
{
    public class FileUploadModel
    {
        public IFormFile FileDetails { get; set; }
    }
}
