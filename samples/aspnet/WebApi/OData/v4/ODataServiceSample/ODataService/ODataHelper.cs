using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;

namespace ODataService
{
    /// <summary>
    /// Helper class to facilitate building an odata service.
    /// </summary>
    public static class ODataHelper
    {
        /// <summary>
        /// Helper method to get the key value from a uri.
        /// Usually used by $ref action to extract the key value from the url in body.
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <param name="request">The request instance in current context</param>
        /// <param name="uri">OData uri that contains the key value</param>
        /// <returns>The key value</returns>
        public static TKey GetKeyValue<TKey>(this HttpRequestMessage request, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            //get the odata path Ex: ~/entityset/key/navigation/$ref
            var serviceRoot = GetServiceRoot(request);
            var odataPath = request.ODataProperties().PathHandler.Parse(request.ODataProperties().Model, serviceRoot, uri.LocalPath);

            var keySegment = odataPath.Segments.OfType<KeyValuePathSegment>().FirstOrDefault();
            if (keySegment == null)
            {
                throw new InvalidOperationException("The link does not contain a key.");
            }

            var value = ODataUriUtils.ConvertFromUriLiteral(keySegment.Value, ODataVersion.V4);
            return (TKey)value;
        }

        private static string GetServiceRoot(HttpRequestMessage request)
        {
            var urlHelper = request.GetUrlHelper() ?? new UrlHelper(request);
            return urlHelper.CreateODataLink(request.ODataProperties().RouteName, request.ODataProperties().PathHandler, new List<ODataPathSegment>());
        }
    }
}
