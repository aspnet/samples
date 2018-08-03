using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;

namespace ProductODataService.Extensions
{
    public class PathAndSlashEscapeODataPathHandler : DefaultODataPathHandler
    {
        private const string EscapedQuote = "'";

        public override ODataPath Parse(IEdmModel model, string serviceRoot, string odataPath)
        {
            if (!odataPath.Contains(EscapedQuote))
            {
                return base.Parse(model, serviceRoot, odataPath);
            }

            var pathBuilder = new StringBuilder();
            var queryStringIndex = odataPath.IndexOf('?');
            if (queryStringIndex == -1)
            {
                EscapeSlashBackslash(odataPath, pathBuilder);
            }
            else
            {
                EscapeSlashBackslash(odataPath.Substring(0, queryStringIndex), pathBuilder);
                pathBuilder.Append(odataPath.Substring(queryStringIndex));
            }
            return base.Parse(model, serviceRoot, pathBuilder.ToString());
        }

        private void EscapeSlashBackslash(string uri, StringBuilder pathBuilder)
        {
            const string slash = "%2F";
            const string backSlash = "%5C";

            var startIndex = uri.IndexOf(EscapedQuote, StringComparison.OrdinalIgnoreCase);
            var endIndex = uri.IndexOf(EscapedQuote, startIndex + EscapedQuote.Length, StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1 || endIndex == -1)
            {
                pathBuilder.Append(uri);
                return;
            }

            endIndex = endIndex + EscapedQuote.Length;
            pathBuilder.Append(uri.Substring(0, startIndex));
            for (var i = startIndex; i < endIndex; ++i)
            {
                switch (uri[i])
                {
                    case '/':
                        pathBuilder.Append(slash);
                        break;
                    case '\\':
                        pathBuilder.Append(backSlash);
                        break;
                    default:
                        pathBuilder.Append(uri[i]);
                        break;
                }
            }
            EscapeSlashBackslash(uri.Substring(endIndex), pathBuilder);
        }
    }
}