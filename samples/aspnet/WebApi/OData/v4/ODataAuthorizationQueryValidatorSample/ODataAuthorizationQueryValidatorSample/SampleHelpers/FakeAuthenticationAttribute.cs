using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace ODataAuthorizationQueryValidatorSample.SampleHelpers
{
    /// <summary>
    /// This filter "authenticates" the user with the values identified in two custom headers "X-Identity" and "X-Roles"
    /// IMPORTANT: This is obviously not a real implementation of an authentication attribute and should never be used
    /// in production code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class FakeAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private static readonly Task _done = Task.FromResult<int>(0);
        private static readonly IEnumerable<AuthenticationHeaderValue> _challenges = new[]{
            new AuthenticationHeaderValue("Basic")
        };

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string userName = context.Request.Headers.GetValues("X-Identity").Single();
            string[] roles = context.Request.Headers.GetValues("X-Roles").Single()
                .Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            context.Principal = new GenericPrincipal(new GenericIdentity(userName), roles);
            return _done;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            if (Thread.CurrentPrincipal != null)
            {
                return _done;
            }
            context.Result = new UnauthorizedResult(_challenges, context.Request);
            return _done;
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}
