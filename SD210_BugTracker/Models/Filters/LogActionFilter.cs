using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SD210_BugTracker.Models.Filters
{
    public class LogActionFilter : ActionFilterAttribute 
    {
        //Before action executes
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var context = new ApplicationDbContext();

            //var log = new ActionLog();

            //log.ActionName = filterContext
            //    .ActionDescriptor
            //    .ActionName;

            //log.ControllerName = filterContext
            //    .ActionDescriptor
            //    .ControllerDescriptor
            //    .ControllerName;

            //context.ActionLogs.Add(log);
            //context.SaveChanges();
            //HttpContext.Current.Response.Write("OnActionExecuting");
        }

        //After action executes
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Trace("OnActionExecuted", filterContext.RouteData);
            //HttpContext.Current.Response.Write("OnActionExecuted");
        }

        //Before ViewResult executes. In other words, before rendering the view.
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //Trace("OnResultExecuting", filterContext.RouteData);
            //HttpContext.Current.Response.Write("OnResultExecuting");
        }


        //After ViewResult executes. In other words, after rendering the view.
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //Trace("OnResultExecuted", filterContext.RouteData);

            //HttpContext.Current.Response.Write("OnResultExecuted");
        }


        private void Trace(string methodName, System.Web.Routing.RouteData routeData)
        {
            //HttpContext.Current.Response.Write("OnResultExecuted");
            //string controllerName = routeData.Values["Controller"].ToString();
            //string actionName = routeData.Values["action"].ToString();

            //HttpContext.Current.Response.Write($"MethodName={methodName}, Controller={controllerName}, Action= {actionName}");

            //string FilePath =MapPath("FILENAME.txt");
            //string FileContent = "Put File Content Here";
            //File.WriteAllText(FilePath, FileContent);
        }
    }
}