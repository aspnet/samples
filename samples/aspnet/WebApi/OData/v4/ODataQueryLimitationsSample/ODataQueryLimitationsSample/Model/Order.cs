using System;

namespace ODataQueryLimitationsSample.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTimeOffset PurchasedOn { get; set; }
    }
}
