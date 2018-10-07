using System;

namespace FlightFinder.Shared
{
    public class CarRentalItinerary
    {
        public string Airport { get; set; }
        public DateTime Pickup { get; set; }
        public DateTime Return { get; set; }
        public decimal Price { get; set; }
        public string CompanyName { get; set; }
        public string Vehicle { get; set; }
    }
}
