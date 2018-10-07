using System;

namespace FlightFinder.Shared
{
    public class CarRental
    {
        public string Company { get; set; }
        public string AirportCode { get; set; }
        public DateTime PickupTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public decimal Price { get; set; }
    }
}
