using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using WebApiAttributeRoutingSample.Models;

// NOTE:
// If you notice the following tests fail, you can
// do the following:
//
// a. Check if the base address of the application is correct
// b. Delete the database specified in the 'ShoppingContext' connection string. This database would get
//    recreated automatically with seed data required for the following tests.
namespace WebApiAttributeRoutingSample.Tests
{
    [TestClass]
    public class Tests
    {
        HttpClient client = null;
        string baseAddress = null;

        public Tests()
        {
            baseAddress = "http://localhost:18965/";

            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
        }

        #region ValuesController

        [TestMethod]
        public void GetAllValues()
        {
            HttpResponseMessage response = client.GetAsync("api/values").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            string[] values = response.Content.ReadAsAsync<string[]>().Result;

            Assert.IsNotNull(values);
            Assert.AreEqual(2, values.Length);
            Assert.AreEqual("value1", values[0]);
            Assert.AreEqual("value2", values[1]);
        }

        #endregion
        
        #region CustomersController

        [TestMethod]
        public void GetAllCustomers()
        {
            HttpResponseMessage response = client.GetAsync("api/customers").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetOrdersOfCustomer()
        {
            HttpResponseMessage response = client.GetAsync("api/customers/1/orders").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region CategoriesController

        [TestMethod]
        public void GetAllCategories()
        {
            HttpResponseMessage response = client.GetAsync("api/categories/all").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetCategoryById()
        {
            HttpResponseMessage response = client.GetAsync("api/categories/1").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetCategoryByName()
        {
            HttpResponseMessage response = client.GetAsync("api/categories/devices").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region OrderManagementController

        [TestMethod]
        public void ApproveOrder()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "api/ordermanagement/1/approveorder");

            HttpResponseMessage response = client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region OrdersController

        [TestMethod]
        public void GetAllOrders()
        {
            HttpResponseMessage response = client.GetAsync("api/orders").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetSingleOrder()
        {
            HttpResponseMessage response = client.GetAsync("api/orders/1").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region ProductsController

        [TestMethod]
        public void GetAllProducts()
        {
            HttpResponseMessage response = client.GetAsync("api/products").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetAllProducts_UsingUriPathExtensionMapping()
        {
            HttpResponseMessage response = client.GetAsync("api/products.xml").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual("application/xml", response.Content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void CreateNewProduct_UriLinkGeneration_BasedOnAttributeRouteName()
        {
            Product prd = new Product();
            prd.Name = "Lync";

            HttpResponseMessage response = client.PostAsJsonAsync<Product>("api/products", prd).Result;

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            Product createdProduct = response.Content.ReadAsAsync<Product>().Result;

            Assert.AreEqual(
                expected: string.Format("{0}api/products/{1}", baseAddress, createdProduct.Id),
                actual: response.Headers.Location.ToString(),
                ignoreCase: true);
        }

        #endregion

        #region Versioning Controllers

        [TestMethod]
        public void GetResponseFromVersionV1()
        {
            HttpResponseMessage response = client.GetAsync("api/v1/countries").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Response from CountriesV1", response.Content.ReadAsAsync<string>().Result);
        }

        [TestMethod]
        public void GetResponseFromVersionV2()
        {
            HttpResponseMessage response = client.GetAsync("api/v2/countries").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Response from CountriesV2", response.Content.ReadAsAsync<string>().Result);
        }

        #endregion
    }
}
