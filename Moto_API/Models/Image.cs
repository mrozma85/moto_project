using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_API.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        [ForeignKey("Ad")]
        public int AdId { get; set; }
    }
}
