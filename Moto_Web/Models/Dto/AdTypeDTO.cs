namespace Moto_Web.Models.Dto
{
    public class AdTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
