using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Client
{
    public class RegisterResponse : ErrorResponse
    {
        public string[] UserNameErrors { get { return GetErrors("model.UserName"); } }
        public string[] PasswordErrors { get { return GetErrors("model.Password"); } }
        public string[] ConfirmPasswordErrors { get { return GetErrors("model.ConfirmPassword");  } }
    }
}
