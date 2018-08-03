using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Client
{
    public class RegisterUser
    {
        public RegisterUser()
        {
        }

        public RegisterUser(string userName, string password)
            : this(userName, password, password)
        {
        }

        public RegisterUser(string userName, string password, string confirmPassword)
        {
            UserName = userName;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
