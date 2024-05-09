using System.ComponentModel.DataAnnotations;

namespace Moto_API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
		public DateTime UpdatedDate { get; set; }
        public string Category_Test { get; set; }
    }
}
