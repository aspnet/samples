// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CustomActionSelector.Server
{
    /// <summary>
    /// Configures a controller to behave different for users that are beta testers. For each action that has HasBetaActionAttribute,
    /// some percentage of beta testers will be directed to the 'beta' version of the action. Which action was chosen is recored as a
    /// response header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BetaTestingBehaviorAttribute : ActionFilterAttribute , IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Services.Replace(typeof(IHttpActionSelector), new BetaTestingActionSelector());
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Request.IsCurrentUserBetaTester())
            {
                object obj;
                bool isBetaAction;
                if (actionExecutedContext.ActionContext.ActionDescriptor.Properties.TryGetValue("IsBetaAction", out obj))
                {
                    isBetaAction = (bool)obj;
                }
                else
                {
                    isBetaAction = false;
                }

                actionExecutedContext.Response.Headers.Add("x-beta-action", isBetaAction.ToString());
            }
        }
    }
}