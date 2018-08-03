using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace CustomModelBinderSample.ModelBinders
{
    public class EmptyCollectionModelBinder : IModelBinder
    {
        IModelBinder innerBinder = null;

        public EmptyCollectionModelBinder(IModelBinder innerBinder)
        {
            this.innerBinder = innerBinder;
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            bool bound = false;

            ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            
            // Following are the scenarios that are handled here
            // a. When no key is provided, let the default behavior be applied.
            // b. When key is provided but with no content, then create an empty collection.
            if ((result != null
                && (result.RawValue == null
                || string.IsNullOrWhiteSpace(result.RawValue.ToString()))))
            {
                bindingContext.Model = CreateEmptyCollection(bindingContext.ModelType);
                bound = true;
            }
            else
            {
                bound = this.innerBinder.BindModel(actionContext, bindingContext);
            }

            return bound;
        }

        private object CreateEmptyCollection(Type modelType)
        {
            Type[] genericArgs = modelType.GetGenericArguments();

            Type baseType = typeof(IEnumerable<>).MakeGenericType(genericArgs[0]);

            if (baseType.IsAssignableFrom(modelType))
            {
                Type type = typeof(List<>).MakeGenericType(genericArgs[0]);

                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
