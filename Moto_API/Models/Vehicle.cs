using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_API.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public int Year { get; set; }
        public int Engine { get; set; }
        public string Color { get; set; }
        public int CompanyId { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("Ad")]
        public int AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
