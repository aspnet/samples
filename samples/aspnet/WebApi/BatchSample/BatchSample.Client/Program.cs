using BatchSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace BatchSample.Client
{
    class Program
    {
        private static readonly string serviceUrl = "http://localhost:65200/api";
        static void Main(string[] args)
        {
            //Sleep for some time to give IIS Express time to start the service
            Thread.Sleep(2000);
            ExecuteBatchRequest().Wait();
            Console.ReadKey();
        }

        private static async Task ExecuteBatchRequest()
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request;
            HttpResponseMessage response;

            request = new HttpRequestMessage(HttpMethod.Get, serviceUrl + "/Customers");
            response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Couldn't get the list of Customers");
                return;
            }
            IList<Customer> customers = await response.Content.ReadAsAsync<IList<Customer>>();

            Customer updatedCustomer = customers.First();
            updatedCustomer.Name = "Peter";
            Customer removedCustomer = customers.ElementAt(1);
            Customer addedCustomer = new Customer { Id = 10, Name = "Name " + 10 };
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            //Create a request to query for customers
            HttpRequestMessage queryCustomersRequest = new HttpRequestMessage(HttpMethod.Get, serviceUrl + "/Customers");
            //Create a message to add a customer
            HttpRequestMessage addCustomerRequest = new HttpRequestMessage(HttpMethod.Post, serviceUrl + "/Customers");
            addCustomerRequest.Content = new ObjectContent<Customer>(addedCustomer, formatter);
            //Create a message to update a customer
            HttpRequestMessage updateCustomerRequest = new HttpRequestMessage(HttpMethod.Put, string.Format(serviceUrl + "/Customers/{0}", updatedCustomer.Id));
            updateCustomerRequest.Content = new ObjectContent<Customer>(updatedCustomer, formatter);
            //Create a message to remove a customer.
            HttpRequestMessage removeCustomerRequest = new HttpRequestMessage(HttpMethod.Delete, string.Format(serviceUrl + "/Customers/{0}", removedCustomer.Id));

            //Create the different parts of the multipart content
            HttpMessageContent queryContent = new HttpMessageContent(queryCustomersRequest);
            HttpMessageContent addCustomerContent = new HttpMessageContent(addCustomerRequest);
            HttpMessageContent updateCustomerContent = new HttpMessageContent(updateCustomerRequest);
            HttpMessageContent removeCustomerContent = new HttpMessageContent(removeCustomerRequest);

            //Create the multipart/mixed message content
            MultipartContent content = new MultipartContent("mixed", "batch_" + Guid.NewGuid().ToString());
            content.Add(queryContent);
            content.Add(addCustomerContent);
            content.Add(updateCustomerContent);
            content.Add(removeCustomerContent);

            //Create the request to the batch service
            HttpRequestMessage batchRequest = new HttpRequestMessage(HttpMethod.Post, serviceUrl + "/batch");
            //Associate the content with the message
            batchRequest.Content = content;

            //Send the message
            HttpResponseMessage batchResponse = await client.SendAsync(batchRequest);
            //Check that the batch response is correct
            if (!batchResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("There was an error executing the batch request.");
                Console.WriteLine(await batchResponse.Content.ReadAsStringAsync());
            }
            //Check that the content in the response is multipart/mixed
            if (!batchResponse.Content.IsMimeMultipartContent("mixed"))
            {
                Console.WriteLine("The returned content is not multipart/mixed");
                Console.WriteLine(await batchResponse.Content.ReadAsStringAsync());
            }
            //Reads the individual parts in the content and loads them in memory
            MultipartMemoryStreamProvider responseContents = await batchResponse.Content.ReadAsMultipartAsync();
            if (!(responseContents.Contents.Count == 4))
            {
                Console.WriteLine("There wrong number of responses came back.");
            }

            HttpResponseMessage queryResponse = await responseContents.Contents[0].ReadAsHttpResponseMessageAsync();
            if (!queryResponse.IsSuccessStatusCode || queryResponse.Content == null)
            {
                Console.WriteLine("The query for the customers failed");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Query result:");
                Console.WriteLine(await queryResponse.Content.ReadAsStringAsync());
                Console.WriteLine();
            }

            HttpResponseMessage addResponse = await responseContents.Contents[1].ReadAsHttpResponseMessageAsync();
            if (!addResponse.IsSuccessStatusCode || addResponse.Content == null)
            {
                Console.WriteLine("The add customer operation failed");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Add result:");
                Console.WriteLine(await addResponse.Content.ReadAsStringAsync());
                Console.WriteLine();
            }

            HttpResponseMessage updateResponse = await responseContents.Contents[2].ReadAsHttpResponseMessageAsync();
            if (!updateResponse.IsSuccessStatusCode || updateResponse.Content == null)
            {
                Console.WriteLine("The update customer operation failed");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Update result:");
                Console.WriteLine(await updateResponse.Content.ReadAsStringAsync());
                Console.WriteLine();
            }

            HttpResponseMessage removeResponse = await responseContents.Contents[3].ReadAsHttpResponseMessageAsync();
            if (!removeResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("The delete customer operation failed");
            }
            else
            {
                Console.WriteLine("The delete operation was successful");
            }
        }
    }
}
