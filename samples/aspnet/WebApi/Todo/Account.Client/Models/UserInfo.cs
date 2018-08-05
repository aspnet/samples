using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Client
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string LoginProvider { get; set; }
        public bool HasRegistered { get; set; }
    }
}
