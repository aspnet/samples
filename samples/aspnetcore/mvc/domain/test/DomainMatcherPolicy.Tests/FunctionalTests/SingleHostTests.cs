using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DomainMatcherPolicy.Tests.FunctionalTests
{
    public class SingleHostTests : IClassFixture<MvcTestFixture<MvcSample.Startup>>
    {
        private readonly MvcTestFixture<MvcSample.Startup> _fixture;

        public SingleHostTests(MvcTestFixture<MvcSample.Startup> fixture)
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
            var request = new HttpRequestMessage(HttpMethod.Get, "api/SingleHost");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("*:*", responseContent);
        }

        [Fact]
        public async Task Get_HostCaseInsensitive()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/SingleHost");

            // Act
            var client = CreateClient("http://LOCALHOST:5003");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("localhost:5003", responseContent);
        }

        [Fact]
        public async Task Get_MatchPort()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/SingleHost");

            // Act
            var client = CreateClient("http://localhost:5000");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("*:5000", responseContent);
        }

        [Fact]
        public async Task Get_MatchDomain()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/SingleHost");

            // Act
            var client = CreateClient("http://127.0.0.1");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("127.0.0.1:*", responseContent);
        }

        [Fact]
        public async Task Get_MatchDomainAndPort()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/SingleHost");

            // Act
            var client = CreateClient("http://127.0.0.1:5000");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("127.0.0.1:5000", responseContent);
        }
    }
}