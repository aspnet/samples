using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODataService.Models
{
    public class RatedProduct: Product
    {
        public int Rating { get; set; }
    }
}
