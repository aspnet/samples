using System;

namespace FlightFinder.Shared
{
    public class FlightSegment
    {
        public string Airline { get; set; }
        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public double DurationHours { get; set; }
        public TicketClass TicketClass { get; set; }
    }
}
