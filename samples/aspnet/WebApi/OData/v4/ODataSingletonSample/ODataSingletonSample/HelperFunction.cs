using System;
using System.Linq;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Core.UriParser.Semantic;

namespace ODataSingletonSample
{
    public static class HelperFunction
    {
        public static TKey GetKeyValue<TKey>(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            // Calculate root Uri
            var rootPath = uri.AbsoluteUri.Substring(0, uri.AbsoluteUri.LastIndexOf('/') + 1);

            var odataUriParser = new ODataUriParser(SingletonEdmModel.GetEdmModel(), new Uri(rootPath), uri);
            var odataPath = odataUriParser.ParsePath();
            var keySegment = odataPath.LastSegment as KeySegment;
            if (keySegment == null)
            {
                throw new InvalidOperationException("The link does not contain a key.");
            }

            return (TKey)keySegment.Keys.First().Value;
        }
    }
}