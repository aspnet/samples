using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;
using ODataActionsSample.Models;

namespace ODataActionsSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Add a custom route convention for non-bindable actions.
            // (Web API does not have a built-in routing convention for non-bindable actions.)
            IList<IODataRoutingConvention> conventions = ODataRoutingConventions.CreateDefault();
            conventions.Insert(0, new NonBindableActionRoutingConvention("NonBindableActions"));

            // Map the OData route.
            config.Routes.MapODataRoute("ODataRoute", "odata", GetModel(), new DefaultODataPathHandler(), conventions);
        }

        // Builds the EDM model for the OData service, including the OData action definitions.
        private static IEdmModel GetModel()
        {
            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
            var moviesEntitySet = modelBuilder.EntitySet<Movie>("Movies");
            moviesEntitySet.EntityType.Ignore(m => m.TimeStamp);    // Don't expose timestamp to clients

            // Now add actions to the EDM.

            // CheckOut
            // URI: ~/odata/Movies(1)/CheckOut
            // Transient action. It is not available when the item is already checked out.
            ActionConfiguration checkout = modelBuilder.Entity<Movie>().TransientAction("CheckOut");

            // Provide a function that returns a link to the action, when the action is available, or
            // returns null when the action is not available.
            checkout.HasActionLink(ctx =>
            {
                Movie movie = ctx.EntityInstance as Movie;

                // Note: In some cases, checking whether the action is available may be relatively expensive.
                // For example, it might require a DB lookup. 

                // Avoid doing expensive checks inside a loop (i.e., when serializing a feed). Instead, simply 
                // mark the action as available, by returning an action link. 

                // The SkipExpensiveAvailabilityChecks flag says whether to skip expensive checks. If this flag 
                // is true AND your availability check is expensive, skip the check and return a link.

                // In this sample, the check is not really expensive, but we honor the flag to show how it works.
                bool createLink = true;
                if (ctx.SkipExpensiveAvailabilityChecks)
                {
                    // Caller asked us to skip the availability check.
                    createLink = true;
                }
                else if (!movie.IsCheckedOut) // Here is the "expensive" check
                {
                    createLink = true;
                }

                if (createLink)
                {
                    // Return the URI of the action.
                    return new Uri(ctx.Url.ODataLink(
                        new EntitySetPathSegment(ctx.EntitySet),
                        new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(movie.ID, ODataVersion.V3)),
                        new ActionPathSegment(checkout.Name)));
                }
                else
                {
                    return null;
                }
            }, followsConventions: true);   // "followsConventions" means the action follows OData conventions.
            checkout.ReturnsFromEntitySet<Movie>("Movies");

            // ReturnMovie
            // URI: ~/odata/Movies(1)/Return
            // Always bindable. If the movie is not checked out, the action is a no-op.
            // Binds to a single entity; no parameters.
            var returnAction = modelBuilder.Entity<Movie>().Action("Return");
            returnAction.ReturnsFromEntitySet<Movie>("Movies");

            // SetDueDate action
            // URI: ~/odata/Movies(1)/SetDueDate
            // Binds to a single entity; takes an action parameter.
            var setDueDate = modelBuilder.Entity<Movie>().Action("SetDueDate");
            setDueDate.Parameter<DateTime>("DueDate");
            setDueDate.ReturnsFromEntitySet<Movie>("Movies");

            // CheckOut action
            // URI: ~/odata/Movies/CheckOut
            // Shows how to bind to a collection, instead of a single entity.
            // This action also accepts $filter queries. For example:
            //     ~/odata/Movies/CheckOut?$filter=Year eq 2005
            var checkOutFromCollection = modelBuilder.Entity<Movie>().Collection.Action("CheckOut");
            checkOutFromCollection.ReturnsCollectionFromEntitySet<Movie>("Movies");

            // CheckOutMany action
            // URI: ~/odata/Movies/CheckOutMany
            // Shows an action that takes a collection parameter.
            ActionConfiguration checkoutMany = modelBuilder.Entity<Movie>().Collection.Action("CheckOutMany");
            checkoutMany.CollectionParameter<int>("MovieIDs");
            checkoutMany.ReturnsCollectionFromEntitySet<Movie>("Movies");

            // CreateMovie action
            // URI: ~/odata/CreateMovie
            // Non-bindable action. You invoke it from the service root.
            ActionConfiguration createMovie = modelBuilder.Action("CreateMovie");
            createMovie.Parameter<string>("Title");
            createMovie.ReturnsFromEntitySet<Movie>("Movies");

            return modelBuilder.GetEdmModel();
        }
    }
}
