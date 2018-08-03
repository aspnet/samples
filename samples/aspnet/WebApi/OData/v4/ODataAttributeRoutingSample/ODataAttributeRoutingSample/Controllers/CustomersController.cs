using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataAttributeRoutingSample.Models;

namespace ODataAttributeRoutingSample.Controllers
{
    public class CustomersController : ODataController
    {
        private static IEnumerable<Customer> Customers = Enumerable.Range(0, 4).Select(i =>
            new Customer
            {
                Id = i,
                Name = "Customer " + i,
                Orders = Enumerable.Range(0, i).Select(j =>
                    new Order
                    {
                        Id = i * 10 + j,
                        Price = (j + 1) * 1.11
                    }
                ).ToList()
            }).ToList();

        [ODataRoute("GetCustomersWithNameContaining(ContainedString={str})")]
        public IEnumerable<Customer> GetSpecialCustomers([FromODataUri]string str)
        {
            return Customers.Where(customer => customer.Name.Contains(str));
        }

        [ODataRoute("Customers({customerId})/Orders({orderId})/Default.UpdateOrderPrice")]
        public Order UpdateOrderPrice(int customerId, int orderId, ODataActionParameters param)
        {
            Order order = Customers.Single(c => c.Id == customerId).Orders.Single(o => o.Id == orderId);
            order.Price = Double.Parse(param["Price"].ToString());
            return order;
        }

        [ODataRoute("Customers({customerId})/Orders/$ref")]
        public Order AddOrderToCustomer(int customerId, [FromBody]Order order)
        {
            Customer customer = Customers.Single(c => c.Id == customerId);
            Order oldOrder = customer.Orders.SingleOrDefault(o => o.Id == order.Id);

            if (oldOrder != null)
            {
                customer.Orders.Remove(oldOrder);
            }

            customer.Orders.Add(order);
            return order;
        }

        [HttpGet]
        [ODataRoute("Customers({customerId})/Default.DeleteOrderFromCustomer(OrderId={id})")]
        public IEnumerable<Order> DeleteOrder(int customerId, int id)
        {
            Customer customer = Customers.Single(c => c.Id == customerId);
            customer.Orders = customer.Orders.Where(o => o.Id != id).ToList();
            return customer.Orders;
        }

        [HttpGet]
        [ODataRoute("Customers")]
        public IEnumerable<Customer> WhateverName()
        {
            return Customers;
        }
    }
}