using System.ComponentModel.DataAnnotations;

namespace Moto_API.Models
{
    public class MainPageImages
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
