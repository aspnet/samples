using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SelectExpandDollarValueSample.Model
{
    public class ShoppingContext : DbContext
    {
        static ShoppingContext()
        {
            Database.SetInitializer<ShoppingContext>(new ShoppingContextInitializer());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        private class ShoppingContextInitializer : DropCreateDatabaseAlways<ShoppingContext>
        {
            protected override void Seed(ShoppingContext context)
            {
                int max = 10;
                IList<Customer> customers = Enumerable.Range(0, max).Select(i => new Customer
                {
                    Name = string.Format("Name {0}", i),
                    Orders = Enumerable.Range(0, i).Select(j => new Order
                    {
                        BillingAddress = new Address
                        {
                            FirstLine = string.Format("First line {0}", i * max + j),
                            SecondLine = string.Format("First line {0}", i * max + j),
                            ZipCode = j * max,
                            City = string.Format("City {0}", i * max + j),
                            State = string.Format("State {0}", i * max + j),
                            Country = string.Format("Country {0}", i * max + j)
                        },
                        PurchaseDate = DateTime.Parse("10/15/2012").Subtract(TimeSpan.FromDays(i * max + j)),
                        OrderItems = Enumerable.Range(0, j).Select(k => new OrderDetail
                        {
                            Name = string.Format("Order item {0}", (i * max * max + j * max + k)),
                            Ammount = i * max * max + j * max + k,
                            Price = i * max * max + j * max + k
                        }).ToList()
                    }).ToList()
                }).ToList();
                for (int i = 0; i < 10; i++)
                {
                    context.Customers.Add(customers[i]);
                }
            }
        }
    }
}
