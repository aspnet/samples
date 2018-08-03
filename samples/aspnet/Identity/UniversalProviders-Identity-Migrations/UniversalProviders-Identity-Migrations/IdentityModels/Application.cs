using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversalProviders_Identity_Migrations
{
    public class Application
    {
        public Application()
        {
        }

        public string ApplicationName { get; set; }
        public System.Guid ApplicationId { get; set; }
        public string Description { get; set; }
    }
}