using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalProviders_Identity_Migrations
{
    public class Role:IRole
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public System.Guid ApplicationId { get; set; }
        public string LoweredRoleName { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}