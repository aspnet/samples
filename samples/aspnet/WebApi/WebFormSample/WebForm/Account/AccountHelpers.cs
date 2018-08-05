using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebForm.Account
{
    public static class AccountHelpers
    {
        public static void Require(IList<string> errors, string fieldValue, string error)
        {
            if (String.IsNullOrEmpty(fieldValue))
            {
                errors.Add(error);
            }
        }

        public static void WriteJsonResponse(HttpResponse response, List<string> errors)
        {
            WriteJsonResponse(response, new { success = errors.Count == 0, errors = errors });
        }

        public static void WriteJsonResponse(HttpResponse response, List<string> errors, string redirect)
        {
            WriteJsonResponse(response, new { success = errors.Count == 0, errors = errors, redirect = redirect });
        }

        public static void WriteJsonResponse(HttpResponse response, object model)
        {
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(model);
            response.Write(json);
            response.End();
        }
    }
}