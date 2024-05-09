using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models.Dto
{
    public class AdNameDTO
	{
        public int Id { get; set; }
        public int AdTypeId { get; set; }
        public AdTypeDTO AdType { get; set; }
        public int VehicleId { get; set; }
        public VehicleDTO Vehicle { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public List<IFormFile> Files { get; set; }

		public ICollection<VehicleImages> ImageByte { get; set; } = new List<VehicleImages>();
		public byte[] FullImageData => ImageByte.FirstOrDefault().FileData;
	}
}
