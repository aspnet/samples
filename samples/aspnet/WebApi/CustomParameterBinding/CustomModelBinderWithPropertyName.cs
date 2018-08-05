using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using CustomParameterBindingSample.Controllers;
using CustomParameterBindingSample.Models;

namespace CustomParameterBindingSample
{
    /// <summary>
    /// A Custom HttpParameterBinding to bind TestItem with a renamed Property
    /// </summary>
    public class CustomModelBinderWithPropertyName : HttpParameterBinding
    {
        public CustomModelBinderWithPropertyName(HttpParameterDescriptor desc)
            : base(desc)
        {
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // read the query and construct the object myself
            TestItemRenameProperty property = new TestItemRenameProperty { Name = actionContext.Request.RequestUri.ParseQueryString()["$Name"] };

            // Set the binding result here
            SetValue(actionContext, property);

            // now, we can return a completed task with no result
            TaskCompletionSource<AsyncVoid> tcs = new TaskCompletionSource<AsyncVoid>();
            tcs.SetResult(default(AsyncVoid));
            return tcs.Task;
        }

        private struct AsyncVoid
        {
        }
    }
}
