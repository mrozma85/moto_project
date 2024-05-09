using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models
{
    public class Ad
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey("AdType")]
        public int AdTypeId { get; set; }
        public AdType AdType { get; set; }


        //[ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<VehicleImages> ImageByte { get; set; } = new List<VehicleImages>();
        public byte[] FullImageData => ImageByte.FirstOrDefault().FileData;
    }
}

