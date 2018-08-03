namespace ExternalEdmModel.Model
{
    public class Order
    {
        public virtual int Id { get; set; }

        public virtual int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual double Total { get; set; }
    }
}
