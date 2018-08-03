// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace CustomActionSelector.Server
{
    /// <summary>
    /// Specifies the alterative action method to invoke for a 'beta user'.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HasBetaTestingActionAttribute : Attribute
    {
        public HasBetaTestingActionAttribute(string methodName)
        {
            MethodName = methodName;
        }

        /// <summary>
        /// Gets the method name of the action to use for a 'beta user'.
        /// </summary>
        public string MethodName
        {
            get;
            private set;
        }
    }
}