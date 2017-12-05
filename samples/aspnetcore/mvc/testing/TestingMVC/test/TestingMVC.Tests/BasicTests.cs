using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace TestingMVC.Tests
{
    public class BasicTests
    {
        [Fact]
        public async Task CanGetHomePage()
        {
            // Arrange
            var webHostBuilder = Program.CreateWebHostBuilder(Array.Empty<string>())
                .UseContentRoot(Path.GetFullPath("../../../../../src/TestingMVC"));

            var server = new TestServer(webHostBuilder);
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
