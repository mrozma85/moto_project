using Microsoft.AspNetCore.Identity;

namespace Moto_API.Models.Dto
{
    public class ApplicationUserDTO : IdentityUser
    {
        public string Name { get; set; }
    }
}
