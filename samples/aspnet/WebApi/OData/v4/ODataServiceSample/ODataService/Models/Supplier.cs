using System.Collections.Generic;

namespace ODataService.Models
{
    public class Supplier
    {
        public Supplier()
        {
            ProductFamilies = new List<ProductFamily>();
            Address = new Address();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public virtual ICollection<ProductFamily> ProductFamilies { get; set; }
    }
}
