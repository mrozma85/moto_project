namespace Moto_Web.Models.Dto
{
    public class AdTypeUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
