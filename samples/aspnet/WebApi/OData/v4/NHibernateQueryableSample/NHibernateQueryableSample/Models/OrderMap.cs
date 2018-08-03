using FluentNHibernate.Mapping;

namespace NHibernateQueryableSample.Models
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Table("Orders");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID");
            Map(x => x.Amount).Column("Amount").Not.Nullable();
            Map(x => x.Quantity).Column("Quantity").Not.Nullable();
        }
    }
}
