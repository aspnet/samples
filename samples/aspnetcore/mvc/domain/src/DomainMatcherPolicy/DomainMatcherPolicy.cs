using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;

namespace DomainMatcherPolicy
{
    public class DomainMatcherPolicy : MatcherPolicy, IEndpointComparerPolicy, INodeBuilderPolicy
    {
        // Run after HTTP methods, but before 'default'.
        public override int Order { get; } = -100;

        public IComparer<Endpoint> Comparer { get; } = new DomainMetadataEndpointComparer();

        public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            return endpoints.Any(e =>
            {
                var hosts = e.Metadata.GetMetadata<DomainAttribute>()?.Hosts;
                if (hosts == null || hosts.Count == 0)
                {
                    return false;
                }

                foreach (var host in hosts)
                {
                    // Don't run policy on endpoints that match everything
                    var key = CreateEdgeKey(host);
                    if (!key.MatchesAll)
                    {
                        return true;
                    }
                }

                return false;
            });
        }

        private static EdgeKey CreateEdgeKey(string host)
        {
            if (host == null)
            {
                return EdgeKey.WildcardEdgeKey;
            }

            var hostParts = host.Split(':');
            if (hostParts.Length == 1)
            {
                return new EdgeKey(hostParts[0], null);
            }
            if (hostParts.Length == 2)
            {
                if (int.TryParse(hostParts[1], out var port))
                {
                    return new EdgeKey(hostParts[0], port);
                }
                else if (string.Equals(hostParts[1], "*", StringComparison.Ordinal))
                {
                    return new EdgeKey(hostParts[0], null);
                }
            }

            throw new InvalidOperationException($"Could not parse host: {host}");
        }

        public IReadOnlyList<PolicyNodeEdge> GetEdges(IReadOnlyList<Endpoint> endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            // The algorithm here is designed to be preserve the order of the endpoints
            // while also being relatively simple. Preserving order is important.

            // First, build a dictionary of all of the domains that are included
            // at this node.
            //
            // For now we're just building up the set of keys. We don't add any endpoints
            // to lists now because we don't want ordering problems.
            var edges = new Dictionary<EdgeKey, List<Endpoint>>();
            for (var i = 0; i < endpoints.Count; i++)
            {
                var endpoint = endpoints[i];
                var hosts = endpoint.Metadata.GetMetadata<DomainAttribute>()?.Hosts.Select(h => CreateEdgeKey(h)).ToArray();
                if (hosts == null || hosts.Length == 0)
                {
                    hosts = new[] { EdgeKey.WildcardEdgeKey };
                }

                for (var j = 0; j < hosts.Length; j++)
                {
                    var contentType = hosts[j];
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

                var endpointKeys = endpoint.Metadata.GetMetadata<DomainAttribute>()?.Hosts.Select(h => CreateEdgeKey(h)).ToArray() ?? Array.Empty<EdgeKey>();
                if (endpointKeys.Length == 0)
                {
                    // OK this means that this endpoint matches *all* domains.
                    // So, loop and add it to all states.
                    foreach (var kvp in edges)
                    {
                        kvp.Value.Add(endpoint);
                    }
                }
                else
                {
                    // OK this endpoint matches specific hosts
                    foreach (var kvp in edges)
                    {
                        // The edgeKey maps to a possible request header value
                        var edgeKey = kvp.Key;

                        for (var j = 0; j < endpointKeys.Length; j++)
                        {
                            var endpointKey = endpointKeys[j];

                            if (edgeKey.Equals(endpointKey))
                            {
                                kvp.Value.Add(endpoint);
                                break;
                            }
                            else if (edgeKey.HasHostWildcard && endpointKey.HasHostWildcard &&
                                edgeKey.Port == endpointKey.Port && edgeKey.MatchHost(endpointKey.Host))
                            {
                                kvp.Value.Add(endpoint);
                                break;
                            }
                        }
                    }
                }
            }

            return edges
                .Select(kvp => new PolicyNodeEdge(kvp.Key, kvp.Value))
                .ToArray();
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
                .Select(e => (domain: (EdgeKey)e.State, destination: e.Destination))
                .OrderBy(e => GetScore(e.domain))
                .ToArray();

            return new DomainPolicyJumpTable(exitDestination, ordered);
        }

        private int GetScore(in EdgeKey key)
        {
            // Higher score == lower priority.
            if (key.MatchesHost && !key.HasHostWildcard && key.MatchesPort)
            {
                return 1; // Has host AND port
            }
            else if (key.MatchesHost && !key.HasHostWildcard)
            {
                return 2; // Has host
            }
            else if (key.MatchesHost && key.MatchesPort)
            {
                return 3; // Has wildcard host AND port
            }
            else if (key.MatchesHost)
            {
                return 4; // Has wildcard host
            }
            else if (key.MatchesPort)
            {
                return 5; // Has port
            }
            else
            {
                return 6; // Has neither
            }
        }

        private class DomainMetadataEndpointComparer : EndpointMetadataComparer<DomainAttribute>
        {
        }

        private class DomainPolicyJumpTable : PolicyJumpTable
        {
            private (EdgeKey domain, int destination)[] _destinations;
            private int _exitDestination;

            public DomainPolicyJumpTable(int exitDestination, (EdgeKey domain, int destination)[] destinations)
            {
                _exitDestination = exitDestination;
                _destinations = destinations;
            }

            public override int GetDestination(HttpContext httpContext)
            {
                var destinations = _destinations;
                for (var i = 0; i < destinations.Length; i++)
                {
                    var destination = destinations[i];

                    if ((!destination.domain.MatchesPort || destination.domain.Port == httpContext.Request.Host.Port) &&
                        destination.domain.MatchHost(httpContext.Request.Host.Host))
                    {
                        return destination.destination;
                    }
                }

                return _exitDestination;
            }
        }

        private readonly struct EdgeKey : IEquatable<EdgeKey>, IComparable<EdgeKey>, IComparable
        {
            private const string WildcardHost = "*";
            internal static readonly EdgeKey WildcardEdgeKey = new EdgeKey(null, null);

            public readonly int? Port;
            public readonly string Host;

            private readonly bool _hasHostWildcard;
            private readonly string _wildcardEndsWith;

            public EdgeKey(string host, int? port)
            {
                Host = host ?? WildcardHost;
                Port = port;

                _hasHostWildcard = Host.StartsWith("*.", StringComparison.Ordinal);
                _wildcardEndsWith = _hasHostWildcard ? Host.Substring(1) : null;
            }

            public bool HasHostWildcard => _hasHostWildcard;

            public bool MatchesHost => !string.Equals(Host, WildcardHost, StringComparison.Ordinal);

            public bool MatchesPort => Port != null;

            public bool MatchesAll => !MatchesHost && !MatchesPort;

            public int CompareTo(EdgeKey other)
            {
                var result = Comparer<string>.Default.Compare(Host, other.Host);
                if (result != 0)
                {
                    return result;
                }

                return Comparer<int?>.Default.Compare(Port, other.Port);
            }

            public int CompareTo(object obj)
            {
                return CompareTo((EdgeKey)obj);
            }

            public bool Equals(EdgeKey other)
            {
                return string.Equals(Host, other.Host, StringComparison.Ordinal) && Port == other.Port;
            }

            public bool MatchHost(string host)
            {
                if (MatchesHost)
                {
                    if (_hasHostWildcard)
                    {
                        return host.EndsWith(_wildcardEndsWith, StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        return string.Equals(host, Host, StringComparison.OrdinalIgnoreCase);
                    }
                }

                return true;
            }

            public override int GetHashCode()
            {
                return (Host?.GetHashCode() ?? 0) ^ (Port?.GetHashCode() ?? 0);
            }

            public override bool Equals(object obj)
            {
                if (obj is EdgeKey key)
                {
                    return Equals(key);
                }

                return false;
            }

            public override string ToString()
            {
                return $"{Host}:{Port?.ToString() ?? "*"}";
            }
        }
    }
}