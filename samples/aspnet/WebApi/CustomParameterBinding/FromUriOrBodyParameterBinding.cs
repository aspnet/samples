using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace CustomParameterBindingSample
{
    /// <summary>
    /// A Custom HttpParameterBinding to bind a complex type either from request body or uri
    /// </summary>
    public class FromUriOrBodyParameterBinding : HttpParameterBinding
    {
        HttpParameterBinding _defaultUriBinding;
        HttpParameterBinding _defaultFormatterBinding;

        public FromUriOrBodyParameterBinding(HttpParameterDescriptor desc)
            : base(desc)
        {
            _defaultUriBinding = new FromUriAttribute().GetBinding(desc);
            _defaultFormatterBinding = new FromBodyAttribute().GetBinding(desc);
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.Request.Content != null && actionContext.Request.Content.Headers.ContentLength > 0)
            {
                // we have something from the body, try that first
                return _defaultFormatterBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
            }
            else
            {
                // we need to read things from uri
                return _defaultUriBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
            }
        }

    }
}
