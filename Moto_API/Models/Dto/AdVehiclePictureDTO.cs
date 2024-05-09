using Moto_API.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Moto_API.Models.Dto
{
    public class AdVehiclePictureDTO
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int VehicleId { get; set; }
    }
}
