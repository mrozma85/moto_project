using Moto_API.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Moto_API.Models
{
    public class AdVehiclePicture
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int VehicleId { get; set; }
    }
}
