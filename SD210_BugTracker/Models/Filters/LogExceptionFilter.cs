using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SD210_BugTracker.Models.Filters
{
    public class LogExceptionFilter : FilterAttribute, IExceptionFilter
    {
        
        public void OnException(ExceptionContext filterContext)
        {
            var log = new ExceptionLog();
            log.ErrorMessage = filterContext.Exception.Message;
            log.Created = DateTime.Now;

            var DbContext = new ApplicationDbContext();
            DbContext.ExceptionLogs.Add(log);
            DbContext.SaveChanges();

            filterContext.ExceptionHandled = true;

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary { { "Exception", filterContext.Exception.Message } }
            };           
        }
    }
}