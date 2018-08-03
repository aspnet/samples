using System;

namespace ODataService.Models
{
    public class Product
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime? SupportedUntil { get; set; }

        public virtual ProductFamily Family { get; set; }
    }
}
