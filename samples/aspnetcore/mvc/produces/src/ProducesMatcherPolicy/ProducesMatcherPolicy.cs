using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Net.Http.Headers;

namespace ProducesMatcherPolicy
{
    public class ProducesMatcherPolicy : MatcherPolicy, IEndpointComparerPolicy, INodeBuilderPolicy
    {
        internal const string Http406EndpointDisplayName = "406 HTTP Unsupported Accept Media Type";
        internal const string AnyContentType = "*/*";

        // Run after HTTP methods, but before 'default'.
        public override int Order { get; } = -1000;

        public IComparer<Endpoint> Comparer { get; } = new ProducesAttributeEndpointComparer();

        public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            return endpoints.Any(e => e.Metadata.GetMetadata<ProducesAttribute>()?.ContentTypes.Count > 0);
        }

        public IReadOnlyList<PolicyNodeEdge> GetEdges(IReadOnlyList<Endpoint> endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            // The algorithm here is designed to be preserve the order of the endpoints
            // while also being relatively simple. Preserving order is important.

            // First, build a dictionary of all of the content-type patterns that are included
            // at this node.
            //
            // For now we're just building up the set of keys. We don't add any endpoints
            // to lists now because we don't want ordering problems.
            var edges = new Dictionary<string, List<Endpoint>>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < endpoints.Count; i++)
            {
                var endpoint = endpoints[i];
                var contentTypes = (IList<string>)endpoint.Metadata.GetMetadata<ProducesAttribute>()?.ContentTypes;
                if (contentTypes == null || contentTypes.Count == 0)
                {
                    contentTypes = new string[] { AnyContentType, };
                }

                for (var j = 0; j < contentTypes.Count; j++)
                {
                    var contentType = contentTypes[j];

                    if (!edges.ContainsKey(contentType))
                    {
                        edges.Add(contentType, new List<Endpoint>());
                    }
                }
            }

            // Now in a second loop, add endpoints to these lists. We've enumerated all of
            // the states, so we want to see which states this endpoint matches.
            for (var i = 0; i < endpoints.Count; i++)
            {
                var endpoint = endpoints[i];
                var contentTypes = (IList<string>)endpoint.Metadata.GetMetadata<ProducesAttribute>()?.ContentTypes ?? Array.Empty<string>();
                if (contentTypes.Count == 0)
                {
                    // OK this means that this endpoint matches *all* content methods.
                    // So, loop and add it to all states.
                    foreach (var kvp in edges)
                    {
                        kvp.Value.Add(endpoint);
                    }
                }
                else
                {
                    // OK this endpoint matches specific content types -- we have to loop through edges here
                    // because content types could either be exact (like 'application/json') or they
                    // could have wildcards (like 'text/*'). We don't expect wildcards to be especially common
                    // with accept, but we need to support it.
                    foreach (var kvp in edges)
                    {
                        // The edgeKey maps to a possible request header value
                        var edgeKey = new MediaType(kvp.Key);

                        for (var j = 0; j < contentTypes.Count; j++)
                        {
                            var contentType = contentTypes[j];

                            var mediaType = new MediaType(contentType);

                            // Example: 'application/json' is subset of 'application/*'
                            // 
                            // This means that when the request has content-type 'application/json' an endpoint
                            // what accept 'application/*' should match.
                            if (edgeKey.IsSubsetOf(mediaType))
                            {
                                kvp.Value.Add(endpoint);

                                // It's possible that a ProducesAttribute defines overlapping wildcards. Don't add an endpoint
                                // to any edge twice
                                break;
                            }
                        }
                    }
                }
            }

            // If after we're done there isn't any endpoint that accepts */*, then we'll synthesize an
            // endpoint that always returns a 404.
            if (!edges.ContainsKey(AnyContentType))
            {
                edges.Add(AnyContentType, new List<Endpoint>()
                {
                    CreateRejectionEndpoint(),
                });
            }

            return edges
                .Select(kvp => new PolicyNodeEdge(kvp.Key, kvp.Value))
                .ToArray();
        }

        private Endpoint CreateRejectionEndpoint()
        {
            return new Endpoint(
                (context) =>
                {
                    context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Task.CompletedTask;
                },
                EndpointMetadataCollection.Empty,
                Http406EndpointDisplayName);
        }

        public PolicyJumpTable BuildJumpTable(int exitDestination, IReadOnlyList<PolicyJumpTableEdge> edges)
        {
            if (edges == null)
            {
                throw new ArgumentNullException(nameof(edges));
            }

            // Since our 'edges' can have wildcards, we do a sort based on how wildcard-ey they
            // are then then execute them in linear order.
            var ordered = edges
                .Select(e => (mediaType: new MediaType((string)e.State), destination: e.Destination))
                .OrderBy(e => GetScore(e.mediaType))
                .ToArray();

            // If any edge matches all content types, then treat that as the 'exit'. This will
            // always happen because we insert a 404 endpoint.
            for (var i = 0; i < ordered.Length; i++)
            {
                if (ordered[i].mediaType.MatchesAllTypes)
                {
                    exitDestination = ordered[i].destination;
                    break;
                }
            }

            return new ProducesPolicyJumpTable(exitDestination, ordered);
        }

        private int GetScore(in MediaType mediaType)
        {
            // Higher score == lower priority - see comments on MediaType.
            if (mediaType.MatchesAllTypes)
            {
                return 4;
            }
            else if (mediaType.MatchesAllSubTypes)
            {
                return 3;
            }
            else if (mediaType.MatchesAllSubTypesWithoutSuffix)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        private class ProducesAttributeEndpointComparer : EndpointMetadataComparer<ProducesAttribute>
        {
            protected override int CompareMetadata(ProducesAttribute x, ProducesAttribute y)
            {
                // Ignore the metadata if it has an empty list of content types.
                return base.CompareMetadata(
                    x?.ContentTypes.Count > 0 ? x : null,
                    y?.ContentTypes.Count > 0 ? y : null);
            }
        }

        private class ProducesPolicyJumpTable : PolicyJumpTable
        {
            private (MediaType mediaType, int destination)[] _destinations;
            private int _exitDestination;

            public ProducesPolicyJumpTable(int exitDestination, (MediaType mediaType, int destination)[] destinations)
            {
                _exitDestination = exitDestination;
                _destinations = destinations;
            }

            public override int GetDestination(HttpContext httpContext)
            {
                var acceptContentType = httpContext.Request.Headers[HeaderNames.Accept];
                if (string.IsNullOrEmpty(acceptContentType))
                {
                    return _exitDestination;
                }

                var mediaTypesWithQuality = new List<MediaTypeSegmentWithQuality>();
                AcceptHeaderParser.ParseAcceptHeader(acceptContentType, mediaTypesWithQuality);
                mediaTypesWithQuality.Sort(_sortFunction);

                var destinations = _destinations;

                // Loop through accept values. Highest quality come first
                for (int i = 0; i < mediaTypesWithQuality.Count; i++)
                {
                    var requestMediaType = new MediaType(mediaTypesWithQuality[i].MediaType);

                    for (var j = 0; j < destinations.Length; j++)
                    {
                        // Don't test */* because we want to find a specified match
                        // Exit destination will handle */*
                        if (!destinations[j].mediaType.MatchesAllTypes
                            && requestMediaType.IsSubsetOf(destinations[j].mediaType))
                        {
                            return destinations[j].destination;
                        }
                    }
                }

                return _exitDestination;
            }

            private static readonly Comparison<MediaTypeSegmentWithQuality> _sortFunction = (left, right) =>
            {
                return left.Quality > right.Quality ? -1 : (left.Quality == right.Quality ? 0 : 1);
            };
        }
    }
}
