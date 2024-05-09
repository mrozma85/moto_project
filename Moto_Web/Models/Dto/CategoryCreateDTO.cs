namespace Moto_Web.Models.Dto
{
    public class CategoryCreateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
