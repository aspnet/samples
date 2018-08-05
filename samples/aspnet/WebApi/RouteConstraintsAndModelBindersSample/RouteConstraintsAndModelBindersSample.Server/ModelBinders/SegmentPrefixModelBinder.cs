using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace RouteConstraintsAndModelBindersSample.Server.ModelBinders
{
    /// <summary>
    /// Used to bind the segment prefix value from the route.
    /// </summary>
    /// <remarks>
    /// If incoming uri's relative path is "/apples:color=red,green/washington;rate=good", "apples" and "washington" 
    /// are segment prefixes.
    /// </remarks>
    public class SegmentPrefixModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            string segmentName = bindingContext.ModelName;
            ValueProviderResult segmentResult = bindingContext.ValueProvider.GetValue(segmentName);
            if (segmentResult == null)
            {
                return false;
            }

            string segmentValue = segmentResult.AttemptedValue;
            if (segmentValue != null)
            {
                bindingContext.Model = segmentValue.Split(new[] { ";" }, 2, StringSplitOptions.None).First();
            }
            else
            {
                bindingContext.Model = segmentValue;
            }
            return true;
        }
    }
}
