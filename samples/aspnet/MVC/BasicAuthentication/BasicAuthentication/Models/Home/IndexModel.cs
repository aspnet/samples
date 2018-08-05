using System.Collections.Generic;
using System.Security.Claims;

namespace BasicAuthentication.Models.Home
{
    public class IndexModel
    {
        public string UserName { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
