// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Web.Http;

namespace CustomActionSelector.Server
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute(
                "Default", 
                "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}