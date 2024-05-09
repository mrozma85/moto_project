using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moto_API.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
