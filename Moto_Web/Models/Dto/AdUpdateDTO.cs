using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models.Dto
{
    public class AdUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AdTypeId { get; set; }

        public int VehicleId { get; set; }
        public VehicleDTO Vehicle { get; set; }

        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
