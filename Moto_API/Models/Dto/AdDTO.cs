using Moto_API.Models.Dto.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Moto_API.Models.Dto
{
    public class AdDTO
    {
        public int Id { get; set; }
        
        public int AdTypeId { get; set; }
        
        public int CategoryId { get; set; }

        public int VehicleId { get; set; }
        public VehicleDTO Vehicle { get; set; }

        public ICollection<Image> Image { get; set; } = new List<Image>();
        public ICollection<VehicleImages> ImageByte { get; set; } = new List<VehicleImages>();

        //public byte[] FullImageData => ImageByte.FirstOrDefault().FileData;

        public string ApplicationUserId { get; set; }
		public ApplicationUserDTO ApplicationUser { get; set; }
	}
}
