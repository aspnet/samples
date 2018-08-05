using System;
using System.Web.Mvc.Filters;
using BasicAuthentication.Results;

namespace BasicAuthentication.Filters
{
    public static class AuthenticationChallengeContextExtensions
    {
        public static void ChallengeWith(this AuthenticationChallengeContext filterContext, string challenge)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            filterContext.Result = new AddChallengeOnUnauthorizedResult(challenge, filterContext.Result);
        }
    }
}