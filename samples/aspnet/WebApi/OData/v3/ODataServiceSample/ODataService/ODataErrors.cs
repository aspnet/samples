// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Microsoft.Data.OData;

namespace ODataService
{
    /// <summary>
    /// A set of useful correctly formatted OData errors.
    /// </summary>
    public static class ODataErrors
    {
        public static ODataError EntityNotFound(string entityName)
        {
            return new ODataError()
            {
                Message = string.Format("Cannot find {0}", entityName), 
                MessageLanguage = "en-US", 
                ErrorCode = "Entity Not Found"
            };
        }

        public static ODataError DeletingLinkNotSupported(string navigation)
        {
            return new ODataError()
            {
                Message = string.Format("Deleting a '{0}' link is not supported.", navigation), 
                MessageLanguage = "en-US", 
                ErrorCode = "Deleting link failed."
            };
        }

        public static ODataError CreatingLinkNotSupported(string navigation)
        {
            return new ODataError()
            {
                Message = string.Format("Creating a '{0}' link is not supported.", navigation), 
                MessageLanguage = "en-US", 
                ErrorCode = "Creating link failed."
            };
        }
    }
}
