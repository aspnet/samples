using FlightFinder.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlightFinder.Server.Controllers
{
    [Route("api/[controller]")]
    public class AirportsController : Controller
    {
        public IEnumerable<Airport> Airports()
        {
            return SampleData.Airports;
        }
    }
}
