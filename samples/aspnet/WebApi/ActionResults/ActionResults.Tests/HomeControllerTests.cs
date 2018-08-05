using System.Text;
using ActionResults.Controllers;
using ActionResults.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActionResults.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void GetText_ReturnsHelloWorldInUtf8()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            OkTextPlainResult result = controller.GetText();

            // Assert
            Assert.AreEqual("Hello, world!", result.Content);
            Assert.AreEqual(Encoding.UTF8, result.Encoding);
        }

        [TestMethod]
        public void GetTextAscii_ReturnsHelloWorldInAscii()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            OkTextPlainResult result = controller.GetTextAscii();

            // Assert

            Assert.AreEqual("Hello, world!", result.Content);
            Assert.AreEqual(Encoding.ASCII, result.Encoding);
        }

        [TestMethod]
        public void GetFile_ReturnsDownloadXmlAsApplicationXml()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            OkFileDownloadResult result = controller.GetFile();

            // Assert
            Assert.AreEqual("application/xml", result.ContentType);
            Assert.AreEqual("Download.xml", result.DownloadFileName);
            Assert.AreEqual("Download.xml", result.LocalPath);
        }
    }
}
