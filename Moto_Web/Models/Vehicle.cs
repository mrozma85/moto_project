namespace Moto_Web.Models
{
	public class Vehicle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public int Year { get; set; }
        public int Engine { get; set; }
        public string Color { get; set; }
        public int CompanyId { get; set; }
        public string Location { get; set; }
        public string Condition { get; set; }
    }
}
