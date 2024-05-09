using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models.Dto
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int VecicleId { get; set; }
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }
    }
}
