using System.Data.Entity;
using System.Net.Http;
using ODataEFBatchSample.Models;

namespace ODataEFBatchSample.Extensions
{
    /// <summary>
    /// Class containing helper methods for associating and retrieving a DbContext instance to/from a given request.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        private static readonly string DB_Context = "Batch_DbContext";

        /// <summary>
        /// Associates a given instance of a DbContext to the current request.
        /// </summary>
        /// <param name="request">The request to will the DbContext instance will be associated.</param>
        /// <param name="context">The DbContext instance to associate with the request.</param>
        public static void SetContext(
            this HttpRequestMessage request,
            ShoppingContext context)
        {
            request.Properties[DB_Context] = context;
        }

        /// <summary>
        /// Retrieves a <see cref="ShoppingContext"/> instance associated with the current request. If no instance is
        /// associated it will create a new one, associate it with the current request and register it for disposal at
        /// the end of the request.
        /// </summary>
        /// <param name="request">The request from which to retrieve the <see cref="CustomerContext"/> instance.</param>
        /// <returns>A <see cref="CustomerContext"/> instance associated with this request.</returns>
        public static ShoppingContext GetContext(this HttpRequestMessage request)
        {
            object customersContext;
            if (request.Properties.TryGetValue(DB_Context, out customersContext))
            {
                return (ShoppingContext)customersContext;
            }
            else
            {
                ShoppingContext context = new ShoppingContext();
                SetContext(request, context);

                request.RegisterForDispose(context);
                return context;
            }
        }
    }
}