using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataVersioningSample.Models
{
    // V2 new complex type
    public class DbAddress
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }
    }
}