using System;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;

namespace ControllerSpecificConfigSample
{
    /// <summary>
    /// This <see cref="Attribute"/> class sets per controller specific configuration. Controllers
    /// marked with this attribute will have a configuration provided by this class.
    /// </summary>
    public class CustomControllerConfigAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            // Insert custom parameter binder as the first in line
            controllerSettings.ParameterBindingRules.Insert(0,
                parameterDescriptor => new MultipleParameterFromBodyParameterBinding(parameterDescriptor));

            // Register an additional plain text media type formatter
            controllerSettings.Formatters.Add(new PlainTextBufferedFormatter());
        }
    }
}
