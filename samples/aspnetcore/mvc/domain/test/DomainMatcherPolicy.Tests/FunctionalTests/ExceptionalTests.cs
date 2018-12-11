using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DomainMatcherPolicy.Tests.FunctionalTests
{
    public class ExceptionalTests : IClassFixture<MvcTestFixture<MvcSample.Startup>>
    {
        private readonly MvcTestFixture<MvcSample.Startup> _fixture;

        public ExceptionalTests(MvcTestFixture<MvcSample.Startup> fixture)
        {
            _fixture = fixture;
        }

        private HttpClient CreateClient(string baseAddress)
        {
            var options = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(baseAddress)
            };
            
            var client = _fixture.CreateClient(options);

            return client;
        }

        [Theory]
        [InlineData("http://localhost")]
        [InlineData("http://localhost:5002")]
        public async Task Get_NoMatches(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Exceptional");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("http://localhost:5000")]
        [InlineData("http://127.0.0.1:5000")]
        public async Task Get_MultipleMatches(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Exceptional");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("http://localhost:5001")]
        [InlineData("http://127.0.0.1:5001")]
        public async Task Get_Match(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Exceptional");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("*:5000,*:5001", responseContent);
        }

        [Theory]
        [InlineData("http://127.0.0.1:5002")]
        [InlineData("http://128.0.0.1:5002")]
        [InlineData("http://127.99.0.1:5002")]
        [InlineData("http://128.99.0.1:5002")]
        public async Task Get_MultipleWildcardMatches(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Exceptional");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("http://localhost:5003")]
        public async Task Get_DontMatchWildcardDomain(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Exceptional");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}