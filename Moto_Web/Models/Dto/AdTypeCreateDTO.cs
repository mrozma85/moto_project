namespace Moto_Web.Models.Dto
{
    public class AdTypeCreateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
