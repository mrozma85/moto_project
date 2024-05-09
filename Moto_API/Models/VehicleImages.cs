using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_API.Models
{
    public class VehicleImages
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }

        [ForeignKey("Ad")]
        public int AdId { get; set; }
    }
}
