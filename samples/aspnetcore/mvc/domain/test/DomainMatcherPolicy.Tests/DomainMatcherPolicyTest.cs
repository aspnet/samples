using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.Routing.Patterns;
using Xunit;

namespace DomainMatcherPolicy.Tests
{
    public class DomainMatcherPolicyTest
    {
        [Fact]
        public void AppliesToEndpoints_EndpointWithoutMetadata_ReturnsFalse()
        {
            // Arrange
            var endpoints = new[] { CreateEndpoint("/", null), };

            var policy = CreatePolicy();

            // Act
            var result = policy.AppliesToEndpoints(endpoints);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AppliesToEndpoints_EndpointWithoutContentTypes_ReturnsFalse()
        {
            // Arrange
            var endpoints = new[]
            {
                CreateEndpoint("/", new DomainAttribute(Array.Empty<string>())),
            };

            var policy = CreatePolicy();

            // Act
            var result = policy.AppliesToEndpoints(endpoints);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AppliesToEndpoints_EndpointHasContentTypes_ReturnsTrue()
        {
            // Arrange
            var endpoints = new[]
            {
                CreateEndpoint("/", new DomainAttribute(Array.Empty<string>())),
                CreateEndpoint("/", new DomainAttribute(new[] { "localhost", })),
            };

            var policy = CreatePolicy();

            // Act
            var result = policy.AppliesToEndpoints(endpoints);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetEdges_GroupsByContentType()
        {
            // Arrange
            var endpoints = new[]
            {
                CreateEndpoint("/", new DomainAttribute(new[] { "*:5000", "*:5001", })),
                CreateEndpoint("/", new DomainAttribute(Array.Empty<string>())),
                CreateEndpoint("/", domainAttribute: null),
                CreateEndpoint("/", new DomainAttribute(new[] { "*.contoso.com:*" })),
                CreateEndpoint("/", new DomainAttribute(new[] { "*.sub.contoso.com:*" })),
                CreateEndpoint("/", new DomainAttribute(new[] { "www.contoso.com:*", })),
                CreateEndpoint("/", new DomainAttribute(new[] { "www.contoso.com:5000", })),
                CreateEndpoint("/", new DomainAttribute(new[]{ "*:*", })),
            };

            var policy = CreatePolicy();

            // Act
            var edges = policy.GetEdges(endpoints);

            var data = edges.OrderBy(e => e.State).ToList();

            // Assert
            Assert.Collection(
                data,
                e =>
                {
                    Assert.Equal("*:*", e.State.ToString());
                    Assert.Equal(new[] { endpoints[1], endpoints[2], endpoints[7], }, e.Endpoints.ToArray());
                },
                e =>
                {
                    Assert.Equal("*:5000", e.State.ToString());
                    Assert.Equal(new[] { endpoints[0], endpoints[1], endpoints[2], }, e.Endpoints.ToArray());
                },
                e =>
                {
                    Assert.Equal("*:5001", e.State.ToString());
                    Assert.Equal(new[] { endpoints[0], endpoints[1], endpoints[2], }, e.Endpoints.ToArray());
                },
                e =>
                {
                    Assert.Equal("*.contoso.com:*", e.State.ToString());
                    Assert.Equal(new[] { endpoints[1], endpoints[2], endpoints[3], endpoints[4], }, e.Endpoints.ToArray());
                },
                e =>
                {
                    Assert.Equal("*.sub.contoso.com:*", e.State.ToString());
                    Assert.Equal(new[] { endpoints[1], endpoints[2], endpoints[4], }, e.Endpoints.ToArray());
                },
                e =>
                {
                    Assert.Equal("www.contoso.com:*", e.State.ToString());
                    Assert.Equal(new[] { endpoints[1], endpoints[2], endpoints[5], }, e.Endpoints.ToArray());
                },
                e =>
                {
                    Assert.Equal("www.contoso.com:5000", e.State.ToString());
                    Assert.Equal(new[] { endpoints[1], endpoints[2], endpoints[6], }, e.Endpoints.ToArray());
                });
        }

        private static RouteEndpoint CreateEndpoint(string template, DomainAttribute domainAttribute)
        {
            var metadata = new List<object>();
            if (domainAttribute != null)
            {
                metadata.Add(domainAttribute);
            }

            return new RouteEndpoint(
                (context) => Task.CompletedTask,
                RoutePatternFactory.Parse(template),
                0,
                new EndpointMetadataCollection(metadata),
                $"test: {template} - {string.Join(", ", domainAttribute?.Hosts ?? Array.Empty<string>())}");
        }

        private static DomainMatcherPolicy CreatePolicy()
        {
            return new DomainMatcherPolicy();
        }
    }
}
