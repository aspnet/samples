using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace ProducesMatcherPolicy.Tests
{
    public class FallbackTests : IClassFixture<MvcTestFixture<MvcSample.Startup>>
    {
        public FallbackTests(MvcTestFixture<MvcSample.Startup> fixture)
        {
            Client = fixture.CreateDefaultClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task Get_DefinedJsonContentType_ReturnsActionForContentType()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Fallback");
            request.Headers.Add(HeaderNames.Accept, "application/json");

            // Act
            var response = await Client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            Assert.Equal(@"""application/json""", responseContent);
        }

        [Fact]
        public async Task Get_DefinedXmlContentType_ReturnsActionForContentType()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Fallback");
            request.Headers.Add(HeaderNames.Accept, "application/xml");

            // Act
            var response = await Client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/xml", response.Content.Headers.ContentType.MediaType);

            Assert.Equal(@"<string>application/xml</string>", responseContent);
        }

        [Fact]
        public async Task Get_NoContentType_ReturnsFallbackAction()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Fallback");

            // Act
            var response = await Client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType.MediaType);

            Assert.Equal(@"*/*", responseContent);
        }

        [Fact]
        public async Task Get_UndefinedContentType_ReturnsFallbackAction()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Fallback");
            request.Headers.Add(HeaderNames.Accept, "cool/beans");

            // Act
            var response = await Client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType.MediaType);

            Assert.Equal(@"*/*", responseContent);
        }

        [Fact]
        public async Task Get_WeightedContentType_ReturnsActionForWeightedContentType()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Fallback");
            request.Headers.Add(HeaderNames.Accept, "application/json, application/xml;q=1.1");

            // Act
            var response = await Client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/xml", response.Content.Headers.ContentType.MediaType);

            Assert.Equal(@"<string>application/xml</string>", responseContent);
        }
    }
}