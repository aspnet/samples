using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightFinder.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FlightFinder.Server.Controllers
{
    [Route("api/[controller]")]
    public class CarRentalSearchController
    {
        public async Task<IEnumerable<CarRentalItinerary>> Search([FromBody] CarRentalSearchCriteria criteria)
        {
            await Task.Delay(500); // Gotta look busy...

            var rng = new Random();
            return Enumerable.Range(0, rng.Next(1, 5)).Select(_ => new CarRentalItinerary
            {
                Airport = criteria.Airport,
                CompanyName = RandomCompany(),
                Pickup = criteria.PickupDate,
                Price = rng.Next(50, 100),
                Return = criteria.ReturnDate,
                Vehicle = RandomCar(criteria.Kind),
            });
        }

        private string RandomCar(CarRentalKind kind)
            => SampleData.RentalCars[kind][new Random().Next(SampleData.RentalCars[kind].Length)];

        private string RandomCompany()
            => SampleData.RentalCarCompanies[new Random().Next(SampleData.RentalCarCompanies.Length)];
    }
}
