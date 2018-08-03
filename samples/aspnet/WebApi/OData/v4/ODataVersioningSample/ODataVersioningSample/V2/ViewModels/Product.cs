using System;

namespace ODataVersioningSample.V2.ViewModels
{
    public class Product
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? ReleaseDate { get; set; }

        public DateTimeOffset? SupportedUntil { get; set; }

        public virtual ProductFamily Family { get; set; }
    }
}