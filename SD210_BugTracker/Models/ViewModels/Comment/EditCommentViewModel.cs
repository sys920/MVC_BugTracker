using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.ViewModels
{
    public class EditCommentViewModel
    {
        public int TId { get; set; }
        public int CID { get; set; }
        public string Comment { get; set; }
    }
}