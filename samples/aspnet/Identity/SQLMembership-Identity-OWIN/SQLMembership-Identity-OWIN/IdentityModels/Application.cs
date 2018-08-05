using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLMembership_Identity_OWIN
{
    public class Application
    {
        public Application()
        {
        }

        public string ApplicationName { get; set; }
        public System.Guid ApplicationId { get; set; }
        public string Description { get; set; }
        public string LoweredApplicationName { get; set; }
    }
}