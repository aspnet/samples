using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace RouteConstraintsAndModelBindersSample.Server.ModelBinders
{
    /// <summary>
    /// Used to bind the segment prefix value from the route.
    /// </summary>
    /// <example>
    /// If [Route["{fruits}/{location}"] is specified and the incoming uri's relative path is
    /// "/apples:color=red,green/washington;rate=good", then in the action's argument list,
    /// <c>[SegmentPrefix] string fruits</c> will have fruits = apples
    /// but <c>string location</c> without this attribute will have location = washington;rate=good.
    /// </example>
    public class SegmentPrefixAttribute : ModelBinderAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            HttpConfiguration config = parameter.Configuration;
            IEnumerable<ValueProviderFactory> valueProviderFactories = GetValueProviderFactories(config);

            return new ModelBinderParameterBinding(parameter, new SegmentPrefixModelBinder(), valueProviderFactories);
        }
    }
}
