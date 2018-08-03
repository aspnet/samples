using System;
using System.Runtime.Serialization;

namespace ODataService.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? ReleaseDate { get; set; }

        public DateTimeOffset?  SupportedUntil { get; set; }

        public virtual ProductFamily Family { get; set; }
    }
}
