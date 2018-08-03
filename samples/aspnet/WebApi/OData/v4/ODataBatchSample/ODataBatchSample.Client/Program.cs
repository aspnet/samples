using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ODataBatchSample.Client.ODataBatchSample.Models;
using CustomersContext = ODataBatchSample.Client.Default.CustomersContext;
using Microsoft.OData.Client;

namespace ODataBatchSample.Client
{
    public class Program
    {
        private static readonly string serviceUrl = "http://localhost:65200/odata";

        public static void Main(string[] args)
        {
            // Sleep for some time to give IIS Express time to start the service
            Thread.Sleep(2000);
            ExecuteBatchRequest().Wait();


            Console.WriteLine("Press Enter to Exit ...");
            Console.ReadKey();
        }

        private static Task ExecuteBatchRequest()
        {
            return Task.Run(() =>
            {
                CustomersContext context = new CustomersContext(new Uri(serviceUrl));
                context.Format.UseJson();
                IList<Customer> customers = context.Customers.ToList();
                Customer customerToAdd = new Customer
                {
                    Id = 5,
                    Name = "New Customer"
                };
                Customer customerToUpdate = customers.Skip(1).First();
                Customer customerToDelete = customers.Skip(2).First();

                context.AddToCustomers(customerToAdd);
                customerToUpdate.Name = "Peter";
                context.UpdateObject(customerToUpdate);
                context.DeleteObject(customerToDelete);

                DataServiceResponse response =
                    context.SaveChanges(SaveChangesOptions.BatchWithSingleChangeset | SaveChangesOptions.ReplaceOnUpdate);
                if (!response.IsBatchResponse)
                {
                    Console.WriteLine("There was an error with the batch request");
                }
                int i = 0;
                foreach (OperationResponse individualResponse in response)
                {
                    Console.WriteLine("Operation {0} status code = {1}", i++, individualResponse.StatusCode);
                }
            });
        }
    }
}
