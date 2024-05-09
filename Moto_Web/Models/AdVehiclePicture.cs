using System.ComponentModel.DataAnnotations;

namespace Moto_Web.Models
{
    public class AdVehiclePicture
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int VehicleId { get; set; }
    }
}
