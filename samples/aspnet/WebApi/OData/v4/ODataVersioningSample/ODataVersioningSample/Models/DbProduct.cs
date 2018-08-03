using System;

namespace ODataVersioningSample.Models
{
    public class DbProduct
    {
        // change from int to long in V2
        public long ID { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? ReleaseDate { get; set; }

        public DateTimeOffset? SupportedUntil { get; set; }

        // V2 property
        public virtual DbProductFamily Family { get; set; }
    }
}