using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.Validation;

namespace Todo.Web.Helpers
{
    public static class ApiControllerExtensions
    {
        public static bool IsValid<TEntity>(this ApiController controller, TEntity entity, out ModelStateDictionary modelState)
        {
            string keyPrefix = String.Empty;
            return controller.IsValid(entity, keyPrefix, out modelState);
        }

        public static bool IsValid<TEntity>(this ApiController controller, TEntity entity, string keyPrefix, out ModelStateDictionary modelState)
        {
            HttpConfiguration configuration = controller.Configuration;
            if (configuration == null)
            {
                throw new InvalidOperationException("Configuration cannot be null");
            }
            IBodyModelValidator validator = configuration.Services.GetBodyModelValidator();
            if (validator != null)
            {
                ModelMetadataProvider metadataProvider = configuration.Services.GetModelMetadataProvider();

                HttpActionDescriptor actionDescriptor = controller.Request.GetActionDescriptor();
                if (actionDescriptor == null)
                {
                    throw new InvalidOperationException("Request must have an action descriptor.");
                }

                HttpActionContext actionContext = new HttpActionContext(controller.ControllerContext, actionDescriptor);
                if (!validator.Validate(entity, typeof(TEntity), metadataProvider, actionContext, keyPrefix))
                {
                    modelState = actionContext.ModelState;
                    return false;
                }
            }

            modelState = null;
            return true;
        }	

    }
}