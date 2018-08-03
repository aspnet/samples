using System;

namespace ODataVersioningSample.V1.ViewModels
{
    public class Product
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime? SupportedUntil { get; set; }
    }
}