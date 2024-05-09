namespace Moto_Web.Models.Dto
{
    public class CategoryUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public string Category_Test { get; set; }
    }
}
