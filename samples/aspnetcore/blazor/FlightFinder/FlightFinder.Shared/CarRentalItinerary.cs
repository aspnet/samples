using System;
using System.Collections.Generic;
using System.Linq;

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

        public class PriceComparer : Comparer<CarRentalItinerary>
        {
            public static readonly PriceComparer Instance = new PriceComparer();

            public override int Compare(CarRentalItinerary x, CarRentalItinerary y)
            {
                return x.Price.CompareTo(y.Price);
            }
        }

        public static (string displayText, IComparer<CarRentalItinerary>)[] GetSortOptions()
        {
            return new (string, IComparer<CarRentalItinerary>)[]
            {
                ("Cheapest", new PriceComparer()),
            };
        }

        public enum SortOrder
        {
            Price,
        }

        public static IEnumerable<CarRentalItinerary> Sort(IEnumerable<CarRentalItinerary> items, SortOrder order)
        {
            return items.OrderBy(i => i.Price);
        }
    }
}
