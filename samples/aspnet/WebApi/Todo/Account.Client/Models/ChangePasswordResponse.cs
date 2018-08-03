using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Client
{
    public class ChangePasswordResponse : ErrorResponse
    {
        public string[] UserNameErrors { get { return GetErrors("model.OldPassword"); } }
        public string[] PasswordErrors { get { return GetErrors("model.NewPassword"); } }
        public string[] ConfirmPasswordErrors { get { return GetErrors("model.ConfirmPassword"); } }
    }
}
