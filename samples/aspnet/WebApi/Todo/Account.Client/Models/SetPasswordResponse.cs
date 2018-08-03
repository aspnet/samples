using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Client
{
    public class SetPasswordResponse : ErrorResponse
    {
        public string[] PasswordErrors { get { return GetErrors("model.NewPassword"); } }
        public string[] ConfirmPasswordErrors { get { return GetErrors("model.ConfirmPassword"); } }
    }
}
