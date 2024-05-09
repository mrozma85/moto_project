using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_API.Models.Dto
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ModelId { get; set; }
        public int CompanyId { get; set; }
        public int Year { get; set; }
        public int Engine { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }

        public Company Company { get; set; }
        public Model Model { get; set; }
    }
}