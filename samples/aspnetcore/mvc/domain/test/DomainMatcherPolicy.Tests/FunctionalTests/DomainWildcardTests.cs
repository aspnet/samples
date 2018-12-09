using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DomainMatcherPolicy.Tests.FunctionalTests
{
    public class DomainWildcardTests : IClassFixture<MvcTestFixture<MvcSample.Startup>>
    {
        private readonly MvcTestFixture<MvcSample.Startup> _fixture;

        public DomainWildcardTests(MvcTestFixture<MvcSample.Startup> fixture)
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
        [InlineData("http://localhost:5001")]
        public async Task Get_CatchAll(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/DomainWildcard");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("*:*", responseContent);
        }

        [Theory]
        [InlineData("http://9000.0.0.1")]
        [InlineData("http://9000.0.0.1:8888")]
        public async Task Get_MatchWildcardDomain(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/DomainWildcard");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("*.0.0.1:*", responseContent);
        }

        [Theory]
        [InlineData("http://127.0.0.1")]
        [InlineData("http://127.0.0.1:8888")]
        public async Task Get_MatchDomain(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/DomainWildcard");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("127.0.0.1:*", responseContent);
        }

        [Theory]
        [InlineData("http://9000.0.0.1:5000")]
        [InlineData("http://9000.0.0.1:5001")]
        public async Task Get_MatchWildcardDomainAndPort(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/DomainWildcard");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("127.0.0.1:5000,127.0.0.1:5001", responseContent);
        }

        [Theory]
        [InlineData("http://www.contoso.com")]
        [InlineData("http://contoso.com")]
        public async Task Get_MatchWildcardDomainAndSubdomain(string baseAddress)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/DomainWildcard");

            // Act
            var client = CreateClient(baseAddress);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("contoso.com:*,*.contoso.com:*", responseContent);
        }
    }
}