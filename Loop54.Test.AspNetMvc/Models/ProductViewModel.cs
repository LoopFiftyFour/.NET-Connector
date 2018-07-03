namespace Loop54.Test.AspNetMvc.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string ImageUrl { get; internal set; }
    }
}
