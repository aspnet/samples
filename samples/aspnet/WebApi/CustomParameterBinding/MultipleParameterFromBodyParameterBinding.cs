using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace CustomParameterBindingSample
{
    /// <summary>
    /// A Custom HttpParameterBinding to bind multiple parameters from request body
    /// </summary>
    public class MultipleParameterFromBodyParameterBinding : HttpParameterBinding
    {
        private const string MultipleBodyParameters = "MultipleBodyParameters";

        public MultipleParameterFromBodyParameterBinding(HttpParameterDescriptor descriptor)
            : base(descriptor)
        {
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            NameValueCollection col = TryReadBody(actionContext.Request);

            if (col == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }

            // Set the binding result here
            SetValue(actionContext, col[Descriptor.ParameterName]);

            // now, we can return a completed task with no result
            TaskCompletionSource<AsyncVoid> tcs = new TaskCompletionSource<AsyncVoid>();
            tcs.SetResult(default(AsyncVoid));
            return tcs.Task;
        }

        NameValueCollection TryReadBody(HttpRequestMessage request)
        {
            object result = null;
            if (!request.Properties.TryGetValue(MultipleBodyParameters, out result))
            {
                // parsing the string like firstname=Hongmei&lastname=Ge
                NameValueCollection collection = new NameValueCollection();
                result = request.Content.ReadAsFormDataAsync().Result;
                request.Properties.Add(MultipleBodyParameters, result);
            }

            return result as NameValueCollection;
        }

        private struct AsyncVoid
        {
        }
    }
}
