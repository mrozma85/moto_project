using System.ComponentModel.DataAnnotations;

namespace Moto_API.Models
{
    public class MainPageDetials
    {
        [Key]
        public int Id { get; set; }
        public string NameTop { get; set; }
        public string NameBottom { get; set; }
        public string OgloszeniaWyroznione { get; set; }
        public string NameStronaGlowna { get; set; }
        public string NameWszystkiePojazdy { get; set; }
        public string NameAdminWszystkiePojazdy { get; set; }
        public string Footer { get; set; }
        public string Login { get; set; }
        public string Register { get; set; }
    }
}
