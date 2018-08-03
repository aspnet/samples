using System;

namespace ODataVersioningSample.V2.ViewModels
{
    public class Product
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime? SupportedUntil { get; set; }

        public virtual ProductFamily Family { get; set; }
    }
}