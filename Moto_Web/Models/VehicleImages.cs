using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models
{
    public class VehicleImages
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public int AdId { get; set; }
    }
}
