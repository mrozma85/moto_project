using DocumentFormat.OpenXml.ExtendedProperties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_API.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }


        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
