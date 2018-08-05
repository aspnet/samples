using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace TouristAttractions.Models
{
    public class TouristAttraction
    {
        public int TouristAttractionId { get; set; }
        public string Name { get; set; }
        public DbGeography Location { get; set; }

        public List<Review> Reviews { get; set; }
    }

    public class Review
    {
        public int ReviewId { get; set; }
        public string Author { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }

        public int TouristAttractionId { get; set; }
        public TouristAttraction TouristAttraction { get; set; }
    }
}
