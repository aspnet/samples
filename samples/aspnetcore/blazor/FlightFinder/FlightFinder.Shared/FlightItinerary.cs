using System.Collections.Generic;
using System.Linq;

namespace FlightFinder.Shared
{
    public class FlightItinerary
    {
        public FlightSegment Outbound { get; set; }
        public FlightSegment Return { get; set; }
        public decimal Price { get; set; }

        public double TotalDurationHours
            => Outbound.DurationHours + Return.DurationHours;

        public string AirlineName => 
            (Outbound.Airline == Return.Airline) ? Outbound.Airline : "Multiple airlines";

        public class PriceComparer : Comparer<FlightItinerary>
        {
            public static readonly PriceComparer Instance = new PriceComparer();

            public override int Compare(FlightItinerary x, FlightItinerary y)
            {
                return x.Price.CompareTo(y.Price);
            }
        }

        public class DurationComparer : Comparer<FlightItinerary>
        {
            public static readonly DurationComparer Instance = new DurationComparer();

            public override int Compare(FlightItinerary x, FlightItinerary y)
            {
                return x.TotalDurationHours.CompareTo(y.TotalDurationHours);
            }
        }

        public static (string displayText, IComparer<FlightItinerary>)[] GetSortOptions()
        {
            return new (string, IComparer<FlightItinerary>)[]
            {
                ("Cheapest", new PriceComparer()),
                ("Shortest", new DurationComparer()),
            };
        }

        public enum SortOrder { Price, Duration };

        public static IEnumerable<FlightItinerary> Sort(IEnumerable<FlightItinerary> items, SortOrder order)
        {
            switch (order)
            {
                case SortOrder.Duration:
                    return items.OrderBy(i => i, PriceComparer.Instance);

                case SortOrder.Price:
                    return items.OrderBy(i => i, DurationComparer.Instance);
            }

            return items;
        }
    }
}
