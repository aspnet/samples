using System;

namespace FlightFinder.Shared
{
    public class CarRentalSearchCriteria
    {
        public string Airport { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public CarRentalKind Kind { get; set; }

        public CarRentalSearchCriteria(string airport)
        {
            Airport = airport;
            PickupDate = DateTime.Now.Date;
            ReturnDate = PickupDate.AddDays(7);
        }
    }
}
