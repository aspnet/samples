using System.Web;
using System.Web.Mvc;

namespace SimpleMembershipMigration
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}