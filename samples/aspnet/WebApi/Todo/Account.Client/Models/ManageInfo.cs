using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Client
{
    public class ManageInfo
    {
        public string LocalLoginProvider { get; set; }

        public string UserName { get; set; }

        public UserLoginInfo[] Logins { get; set; }

        public ExternalLogin[] ExternalLoginProviders { get; set; }
    }
}
