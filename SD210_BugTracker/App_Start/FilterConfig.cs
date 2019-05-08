using SD210_BugTracker.Models.Filters;
using System.Web;
using System.Web.Mvc;

namespace SD210_BugTracker
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthorizationFilter());
            //filters.Add(new LogActionFilter());
            filters.Add(new LogExceptionFilter());
        }
    }
}
