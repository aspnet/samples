using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiAttributeRoutingSample.Models
{
    public class ShoppingContextInitializer : DropCreateDatabaseAlways<ShoppingContext>
    {
        protected override void Seed(ShoppingContext context)
        {
            Category devices = new Category();
            devices.Name = "Devices";

            //--------------

            Supplier apple = new Supplier();
            apple.CompanyName = "Apple";
            apple.City = "Cupertino";
            apple.State = "CA";
            apple.Country = "USA";
            apple.PostalCode = "95014";

            Product ipad = new Product();
            ipad.Name = "IPad";
            ipad.Category = devices;
            
            apple.Products.Add(ipad);

            context.Suppliers.Add(apple);

            //--------------

            Supplier microsoft = new Supplier();
            microsoft.CompanyName = "Microsoft";
            microsoft.City = "Redmond";
            microsoft.State = "WA";
            microsoft.Country = "USA";
            microsoft.PostalCode = "98052";

            Product surface = new Product();
            surface.Name = "Surface";
            surface.Category = devices;

            Product xboxOne = new Product();
            xboxOne.Name = "Xbox One";
            xboxOne.Category = devices;

            microsoft.Products.Add(surface);
            microsoft.Products.Add(xboxOne);

            context.Suppliers.Add(microsoft);

            //---------------

            Shipper fedex = new Shipper();
            fedex.CompanyName = "Fedex";

            Shipper ups = new Shipper();
            ups.CompanyName = "UPS";

            context.Shippers.Add(fedex);
            context.Shippers.Add(ups);

            //---------------

            Customer john = new Customer();
            john.Name = "John";
            john.City = "Bellevue";
            john.State = "WA";
            john.Country = "USA";
            john.PostalCode = "98006";

            Order order1 = new Order();
            order1.Shipper = fedex;

            Order_Detail order1detail1 = new Order_Detail();
            order1detail1.Order = order1;
            order1detail1.Product = ipad;
            order1detail1.Quantity = 1;
            order1detail1.UnitPrice = 499.00M;

            order1.Order_Details.Add(order1detail1);

            Customer michael = new Customer();
            michael.Name = "Michael";
            michael.City = "Norman";
            michael.State = "OK";
            michael.Country = "USA";
            michael.PostalCode = "73072";

            Order order2 = new Order();
            order2.Shipper = ups;

            Order_Detail order2detail1 = new Order_Detail();
            order2detail1.Order = order2;
            order2detail1.Product = surface;
            order2detail1.Quantity = 1;
            order2detail1.UnitPrice = 699.00M;

            order2.Order_Details.Add(order2detail1);

            john.Orders.Add(order1);
            michael.Orders.Add(order2);

            context.Customers.Add(john);
            context.Customers.Add(michael);

            //---------------

            context.SaveChanges();
        }
    }
}