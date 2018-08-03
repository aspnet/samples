using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;

namespace CustomModelBinderSample.ModelBinders
{
    public class EmptyCollectionModelBinderProvider : ModelBinderProvider
    {
        private CollectionModelBinderProvider originalProvider = null;

        public EmptyCollectionModelBinderProvider(CollectionModelBinderProvider originalProvider)
        {
            this.originalProvider = originalProvider;
        }

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            // get the default implementation of provider for handling collections
            IModelBinder originalBinder = originalProvider.GetBinder(configuration, modelType);

            if (originalBinder != null)
            {
                return new EmptyCollectionModelBinder(innerBinder: originalBinder);
            }

            return null;
        }
    }
}
