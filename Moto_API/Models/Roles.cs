namespace Moto_API.Models
{
    public class Roles
    {
        public ApplicationUser User { get; set; }
        public IList<string> RolesNames { get; set; }
    }
}
