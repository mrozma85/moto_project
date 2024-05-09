using DocumentFormat.OpenXml.ExtendedProperties;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_Web.Models
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
