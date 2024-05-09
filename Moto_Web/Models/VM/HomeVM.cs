using Moto_Web.Models.Dto;

namespace Moto_Web.Models.VM
{
    public class HomeVM
    {
        public HomeVM()
        {
            Ad = new List<AdDTO>();
        }

        public List<AdDTO> Ad { get; set; }

        public List<MainPageDetails> Main { get; set; }

        public List<AdDTO> AdName { get; set; }
        public List<CategoryDTO> Category { get; set; }
        public LoginRequestDTO LoginRequestDTO { get; set; }
    }
}
