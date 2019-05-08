using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Created { get; set; }
    }
}