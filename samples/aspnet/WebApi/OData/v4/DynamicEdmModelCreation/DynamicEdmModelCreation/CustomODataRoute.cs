using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.OData.Routing;

namespace DynamicEdmModelCreation
{
    public class CustomODataRoute : ODataRoute
    {
        private static readonly string _escapedHashMark = Uri.HexEscape('#');
        private static readonly string _escapedQuestionMark = Uri.HexEscape('?');

        private bool _canGenerateDirectLink;

        public CustomODataRoute(string routePrefix, ODataPathRouteConstraint pathConstraint)
            : base(routePrefix, pathConstraint)
        {
            _canGenerateDirectLink = routePrefix != null && RoutePrefix.IndexOf('{') == -1;
        }

        public override IHttpVirtualPathData GetVirtualPath(
            HttpRequestMessage request,
            IDictionary<string, object> values)
        {
            if (values == null || !values.Keys.Contains(HttpRoute.HttpRouteKey, StringComparer.OrdinalIgnoreCase))
            {
                return null;
            }

            object odataPathValue;
            if (!values.TryGetValue(ODataRouteConstants.ODataPath, out odataPathValue))
            {
                return null;
            }

            string odataPath = odataPathValue as string;
            if (odataPath != null)
            {
                return GenerateLinkDirectly(request, odataPath) ?? base.GetVirtualPath(request, values);
            }

            return null;
        }

        internal HttpVirtualPathData GenerateLinkDirectly(HttpRequestMessage request, string odataPath)
        {
            HttpConfiguration configuration = request.GetConfiguration();
            if (configuration == null || !_canGenerateDirectLink)
            {
                return null;
            }

            string dataSource = request.Properties[Constants.ODataDataSource] as string;
            string link = CombinePathSegments(RoutePrefix, dataSource);
            link = CombinePathSegments(link, odataPath);
            link = UriEncode(link);

            return new HttpVirtualPathData(this, link);
        }

        private static string CombinePathSegments(string routePrefix, string odataPath)
        {
            return string.IsNullOrEmpty(routePrefix)
                ? odataPath
                : (string.IsNullOrEmpty(odataPath) ? routePrefix : routePrefix + '/' + odataPath);
        }

        private static string UriEncode(string str)
        {
            string escape = Uri.EscapeUriString(str);
            escape = escape.Replace("#", _escapedHashMark);
            escape = escape.Replace("?", _escapedQuestionMark);
            return escape;
        }
    }
}