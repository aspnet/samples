using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasicAuthentication.Results
{
    public class AddChallengeOnUnauthorizedResult : ActionResult
    {
        public AddChallengeOnUnauthorizedResult(string challenge, ActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        public string Challenge { get; private set; }

        public ActionResult InnerResult { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            InnerResult.ExecuteResult(context);
            HttpResponseBase response = context.HttpContext.Response;

            if (response.StatusCode == 401)
            {
                string[] existingChallenges = response.Headers.GetValues("WWW-Authenticate");

                if (existingChallenges == null || !existingChallenges.Any(c => c == Challenge))
                {
                    response.Headers.Add("WWW-Authenticate", Challenge);
                }
            }
        }
    }
}
