using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Client
{
    public class ErrorResponse
    {
        Dictionary<string, string[]> modelState = new Dictionary<string, string[]>();
        string[] errors;

        public string Message { get; set; }
        public Dictionary<string, string[]> ModelState { get { return modelState; } }

        public string[] ErrorMessages { get { return GetErrors(""); } }
        public bool HasErrors { get { return AllErrors.Length > 0 || Message != null; } }

        public string[] GetErrors(string key)
        {
            string[] errors;
            return ModelState.TryGetValue(key, out errors) ? errors : new string[0];
        }

        public string[] AllErrors
        {
            get
            {
                if (errors == null)
                {
                    List<string> errorsList = new List<string>();
                    foreach (string[] modelErrors in modelState.Values)
                    {
                        errorsList.AddRange(modelErrors);
                    }
                    if (Message != null)
                    {
                        errorsList.Add(Message);
                    }
                    errors = errorsList.ToArray();
                }
                return errors;
            }
        }
    }
}
