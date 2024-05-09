using Moto_API.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Moto_API.Models
{
    public class Ad
    {
        [Key]
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)] <<<< ==== id mozna wpisac samemu i jest dalej niepowtarzalne
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        
        public Vehicle Vehicle { get; set; }
        public ICollection<Image> Image { get; set; } = new List<Image>();
        public ICollection<VehicleImages> ImageByte { get; set; } = new List<VehicleImages>(); 

        public int AdTypeId { get; set; }
        public AdType AdType { get; set; }
        
        public int CategoryId { get; set; } 
        public Category Category { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
