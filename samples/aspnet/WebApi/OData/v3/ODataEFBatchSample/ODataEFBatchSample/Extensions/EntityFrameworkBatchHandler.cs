using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Batch;
using ODataEFBatchSample.Models;

namespace ODataEFBatchSample.Extensions
{
    /// <summary>
    /// Custom batch handler specialized to execute batch changeset in OData $batch requests with transactions.
    /// The requests will be executed in the order they arrive, that means that the client is responsible for
    /// correctly ordering the operations to satisfy referential constraints.
    /// </summary>
    public class EntityFrameworkBatchHandler : DefaultODataBatchHandler
    {
        public EntityFrameworkBatchHandler(HttpServer httpServer)
            : base(httpServer)
        {
        }

        /// <summary>
        /// Executes the batch request and associates a <see cref="ShoppingContext"/>instance with all the requests of 
        /// a single changeset and wraps the execution of the whole changeset within a transaction.
        /// </summary>
        /// <param name="requests">The <see cref="ODataBatchRequestItem"/> instances of this batch request.</param>
        /// <param name="cancellation">The <see cref="CancellationToken"/> associated with the request.</param>
        /// <returns>The list of responses associated with the batch request.</returns>
        public async override Task<IList<ODataBatchResponseItem>> ExecuteRequestMessagesAsync(
            IEnumerable<ODataBatchRequestItem> requests,
            CancellationToken cancellation)
        {
            if (requests == null)
            {
                throw new ArgumentNullException("requests");
            }

            IList<ODataBatchResponseItem> responses = new List<ODataBatchResponseItem>();
            try
            {
                foreach (ODataBatchRequestItem request in requests)
                {
                    OperationRequestItem operation = request as OperationRequestItem;
                    if (operation != null)
                    {
                        responses.Add(await request.SendRequestAsync(Invoker, cancellation));
                    }
                    else
                    {
                        await ExecuteChangeSet((ChangeSetRequestItem)request, responses, cancellation);
                    }
                }
            }
            catch
            {
                foreach (ODataBatchResponseItem response in responses)
                {
                    if (response != null)
                    {
                        response.Dispose();
                    }
                }
                throw;
            }

            return responses;
        }

        private async Task ExecuteChangeSet(
            ChangeSetRequestItem changeSet,
            IList<ODataBatchResponseItem> responses,
            CancellationToken cancellation)
        {
            ChangeSetResponseItem changeSetResponse;

            // Create a new ShoppingContext instance, associate it with each of the requests, start a new
            // transaction, execute the changeset and then commit or rollback the transaction depending on
            // whether the responses were all successful or not.
            using (ShoppingContext context = new ShoppingContext())
            {
                foreach (HttpRequestMessage request in changeSet.Requests)
                {
                    request.SetContext(context);
                }

                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    changeSetResponse = (ChangeSetResponseItem)await changeSet.SendRequestAsync(Invoker, cancellation);
                    responses.Add(changeSetResponse);

                    if (changeSetResponse.Responses.All(r => r.IsSuccessStatusCode))
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}