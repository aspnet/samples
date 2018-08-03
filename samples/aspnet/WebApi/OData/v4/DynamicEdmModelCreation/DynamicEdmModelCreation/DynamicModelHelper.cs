using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Batch;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using DynamicEdmModelCreation.DataSource;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace DynamicEdmModelCreation
{
    public static class DynamicModelHelper
    {
        public static ODataRoute CustomMapODataServiceRoute(
            HttpRouteCollection routes,
            string routeName,
            string routePrefix)
        {
            IList<IODataRoutingConvention> routingConventions = ODataRoutingConventions.CreateDefault();
            routingConventions.Insert(0, new MatchAllRoutingConvention());
            return CustomMapODataServiceRoute(
                routes,
                routeName,
                routePrefix,
                GetModelFuncFromRequest(),
                new DefaultODataPathHandler(),
                routingConventions,
                batchHandler: null);
        }

        private static ODataRoute CustomMapODataServiceRoute(
            HttpRouteCollection routes,
            string routeName,
            string routePrefix,
            Func<HttpRequestMessage, IEdmModel> modelProvider,
            IODataPathHandler pathHandler,
            IEnumerable<IODataRoutingConvention> routingConventions,
            ODataBatchHandler batchHandler)
        {
            if (!string.IsNullOrEmpty(routePrefix))
            {
                int prefixLastIndex = routePrefix.Length - 1;
                if (routePrefix[prefixLastIndex] == '/')
                {
                    routePrefix = routePrefix.Substring(0, routePrefix.Length - 1);
                }
            }

            if (batchHandler != null)
            {
                batchHandler.ODataRouteName = routeName;
                string batchTemplate = string.IsNullOrEmpty(routePrefix)
                    ? ODataRouteConstants.Batch
                    : routePrefix + '/' + ODataRouteConstants.Batch;
                routes.MapHttpBatchRoute(routeName + "Batch", batchTemplate, batchHandler);
            }

            CustomODataPathRouteConstraint routeConstraint = new CustomODataPathRouteConstraint(
                pathHandler,
                modelProvider,
                routeName,
                routingConventions);
            CustomODataRoute odataRoute = new CustomODataRoute(routePrefix, routeConstraint);
            routes.Add(routeName, odataRoute);

            return odataRoute;
        }
    
        private static Func<HttpRequestMessage, IEdmModel> GetModelFuncFromRequest()
        {
            return request =>
            {
                string odataPath = request.Properties[Constants.CustomODataPath] as string ?? string.Empty;
                string[] segments = odataPath.Split('/');
                string dataSource = segments[0];
                request.Properties[Constants.ODataDataSource] = dataSource;
                IEdmModel model = DataSourceProvider.GetEdmModel(dataSource);
                request.Properties[Constants.CustomODataPath] = string.Join("/", segments, 1, segments.Length - 1);
                return model;
            };
        }
    }
}