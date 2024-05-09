using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int AdId { get; set; }
    }
}
