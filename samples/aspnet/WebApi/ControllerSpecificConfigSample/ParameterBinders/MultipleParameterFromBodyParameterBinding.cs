using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace ControllerSpecificConfigSample
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

        public override async Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            NameValueCollection col = await ReadBodyAsync(actionContext.Request);
            if (col == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Set the binding result here
            SetValue(actionContext, col[Descriptor.ParameterName]);
        }

        async Task<NameValueCollection> ReadBodyAsync(HttpRequestMessage request)
        {
            object result = null;
            if (!request.Properties.TryGetValue(MultipleBodyParameters, out result))
            {
                result = await request.Content.ReadAsFormDataAsync();
                request.Properties.Add(MultipleBodyParameters, result);
            }

            return result as NameValueCollection;
        }
    }
}
