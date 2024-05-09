using System.ComponentModel.DataAnnotations;

namespace Moto_API.Models
{
    public class AdType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
