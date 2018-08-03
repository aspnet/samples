using FluentNHibernate.Mapping;

namespace NHibernateQueryableSample.Models
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Table("Customers");
            LazyLoad();
            Id(x => x.Name).GeneratedBy.Assigned().Column("Name");
            Map(x => x.Address).Column("Address").Not.Nullable().Length(127);
            Map(x => x.City).Column("City").Not.Nullable().Length(127);
            Map(x => x.State).Column("State").Not.Nullable().Length(127);
            HasMany(x => x.Orders).KeyColumn("CustomerName").Cascade.All();
        }
    }
}
