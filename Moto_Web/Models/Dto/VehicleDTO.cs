using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
﻿using Moto_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models.Dto
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nazwa ogłoszenia jest wymagana")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Opis pojazdu jest wymagany")]
        public string Description { get; set; }
        [Range(0, 1000000, ErrorMessage = "Cena nie mniejsz niż 0")]
        [Required(ErrorMessage = "Proszę podać cene")]
        public double Price { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        [Range(1900, 2024, ErrorMessage = "Rok produkcji od 1900 do 2024")]
        [Required(ErrorMessage = "Proszę podać rok pojazdu")]
        public int Year { get; set; }
        [Range(0, 8000, ErrorMessage = "Pojemność silnika od 0 do 8000 (cm3)")]
        [Required(ErrorMessage = "Proszę podać cene")]
        public int Engine { get; set; }
        public string Color { get; set; }
        public int CompanyId { get; set; }
        public string Location { get; set; }
        public string Condition { get; set; }
    }
}
