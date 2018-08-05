using System.Collections.Generic;

namespace BasicAuthentication.Models
{
    public class HomeModel
    {
        public string UserName { get; set; }

        public IEnumerable<ClaimModel> Claims { get; set; }
    }
}
