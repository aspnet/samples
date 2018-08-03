using System;
using System.Collections.Generic;

namespace ODataQueryableSample.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public DateTimeOffset BirthTime { get; set; }

        public List<Order> Orders { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
