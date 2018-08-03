using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Client
{
    public class RegisterExternalResponse : ErrorResponse
    {
        public string[] UserNameErrors { get { return GetErrors("model.UserName"); } }
    }
}
