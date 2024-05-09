using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models.Dto
{
    public class AdCreateDTO
    {
        public int AdId { get; set; }
        [Required(ErrorMessage = "Proszę wybrać typ ogłoszenia")]
        public int AdTypeId { get; set; }
        public AdTypeDTO AdType{ get; set; }

        public int VehicleId { get; set; }
        public VehicleDTO Vehicle { get; set; }
        [Required(ErrorMessage = "Proszę wybrać kategorię pojazdu")]
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }

        public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
	}
}
