
namespace NHibernateQueryableSample.Models
{
    public class Order
    {
        public Order() { }
        public virtual int ID { get; set; }
        public virtual int Amount { get; set; }
        public virtual int Quantity { get; set; }
    }
}
