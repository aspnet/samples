using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ODataService.Extensions
{
    /// <summary>
    /// Throws bad request error when model state is invalid
    /// </summary>
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response =
                    actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest,
                        actionContext.ModelState);
            }
        }
    }
}
