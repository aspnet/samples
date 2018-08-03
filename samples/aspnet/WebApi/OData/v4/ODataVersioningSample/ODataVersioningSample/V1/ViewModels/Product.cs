using System;

namespace ODataVersioningSample.V1.ViewModels
{
    public class Product
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? ReleaseDate { get; set; }

        public DateTimeOffset? SupportedUntil { get; set; }
    }
}