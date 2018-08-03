using System.ComponentModel.DataAnnotations;

namespace ODataEtagSample.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        [ConcurrencyCheck]
        public int Version { get; set; }
    }
}
