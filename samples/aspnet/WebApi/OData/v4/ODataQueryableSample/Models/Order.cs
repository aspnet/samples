namespace ODataQueryableSample.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public Origin Origin { get; set; }
    }

    /// <summary>
    /// Complex Type
    /// </summary>
    public class Origin
    {
        public string City { get; set; }

        public int PostCode { get; set; }
    }
}
