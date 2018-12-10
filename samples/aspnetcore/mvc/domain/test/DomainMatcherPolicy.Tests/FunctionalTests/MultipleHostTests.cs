using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DomainMatcherPolicy.Tests.FunctionalTests
{
    public class MultipleHostTests : IClassFixture<MvcTestFixture<MvcSample.Startup>>
    {
        private readonly MvcTestFixture<MvcSample.Startup> _fixture;

        public MultipleHostTests(MvcTestFixture<MvcSample.Startup> fixture)
        {
            _fixture = fixture;
        }

        private HttpClient CreateClient(string baseAddress)
        {
            var client = _fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(baseAddress)
            });

            return client;
        }

        [Theory]
        [InlineData("http://localhost")]
        [InlineData("http://localhost:5002")]
        public async Task Get_CatchAll(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/MultipleHost");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("*:*", responseContent);
        }

        [Theory]
        [InlineData("http://localhost:5000")]
        [InlineData("http://localhost:5001")]
        [InlineData("http://127.0.0.1")]
        public async Task Get_IP_OrPort(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/MultipleHost");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("*:5000,*:5001,127.0.0.1:*", responseContent);
        }

        [Theory]
        [InlineData("http://127.0.0.1:5000")]
        [InlineData("http://127.0.0.1:5001")]
        public async Task Get_IP_AndPort(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/MultipleHost");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("127.0.0.1:5000,127.0.0.1:5001", responseContent);
        }
    }
}