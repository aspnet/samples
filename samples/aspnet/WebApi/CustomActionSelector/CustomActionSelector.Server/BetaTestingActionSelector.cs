// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;

namespace CustomActionSelector.Server
{
    /// <summary>
    /// Randomly selects a 'beta-testing' version of actions for users that are part of the beta program.
    /// </summary>
    public class BetaTestingActionSelector : ApiControllerActionSelector
    {
        private readonly ConcurrentDictionary<HttpActionDescriptor, HttpActionDescriptor> _betaActions = new ConcurrentDictionary<HttpActionDescriptor, HttpActionDescriptor>();

        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            var action = base.SelectAction(controllerContext);

            if (!controllerContext.Request.IsCurrentUserBetaTester() || DateTime.Now.Second % 2 == 0)
            {
                // Send the user to the 'normal' action
                return action;
            }
            else
            {
                // This user is in the beta program and was selected to see a different version of the action.

                HttpActionDescriptor betaAction;
                if (!_betaActions.TryGetValue(action, out betaAction))
                {
                    betaAction = CreateBetaAction(action);
                    _betaActions.TryAdd(action, betaAction);
                }

                if (betaAction == null)
                {
                    return action;
                }
                else
                {
                    return betaAction;
                }
            }
        }

        private HttpActionDescriptor CreateBetaAction(HttpActionDescriptor originalAction)
        {
            var attributes = originalAction.GetCustomAttributes<HasBetaTestingActionAttribute>().OfType<HasBetaTestingActionAttribute>();
            if (!attributes.Any())
            {
                return null;
            }

            var methodName = attributes.First().MethodName;
            var actionMethod = originalAction.ControllerDescriptor.ControllerType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (actionMethod == null)
            {
                return null;
            }
           
            var actionDescriptor = new ReflectedHttpActionDescriptor(originalAction.ControllerDescriptor, actionMethod);
            actionDescriptor.Properties.TryAdd("IsBetaAction", true);
            return actionDescriptor;
        }
    }
}