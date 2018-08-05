
namespace CustomModelBinderSample.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public ProductCategoryType Category { get; set; }
    }
}
