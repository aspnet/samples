
using System;

// Duplicate server-side model class
namespace Client
{
    public class MyData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Decimal { get; set; }

        public double Double { get; set; }

        public long Long { get; set; }

        public TimeSpan TimeSpan { get; set; }
    }
}