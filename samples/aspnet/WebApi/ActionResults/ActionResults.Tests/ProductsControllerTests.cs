using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using ActionResults.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActionResults.Tests
{
    [TestClass]
    public class ProductsControllerTests
    {
        [TestMethod]
        public void Get_IfCatalogDoesNotContainId_ReturnsNotFound()
        {
            // Arrange
            IDictionary<int, string> catalog = new Dictionary<int, string>();
            int idNotInCatalog = 1;
            Assert.IsFalse(catalog.ContainsKey(idNotInCatalog)); // Guard
            ProductsController controller = new ProductsController(catalog);

            // Act
            IHttpActionResult result = controller.Get(idNotInCatalog);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Get_IfCatalogContainsId_ReturnsSpecifiedValue()
        {
            // Arrange
            int idInCatalog = 1;
            string expectedValue = "Product";
            IDictionary<int, string> catalog = new Dictionary<int, string>
            {
                { idInCatalog, expectedValue }
            };
            Assert.IsTrue(catalog.ContainsKey(idInCatalog)); // Guard
            ProductsController controller = new ProductsController(catalog);

            // Act
            IHttpActionResult result = controller.Get(idInCatalog);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            OkNegotiatedContentResult<string> okResult = (OkNegotiatedContentResult<string>)result;
            Assert.AreEqual(expectedValue, okResult.Content);
        }
    }
}
