using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.ViewModels
{
    public class DeleteFileUploadedViewModel
    {
        public int Id { get; set; }
        public string FilePath { get; set; }       
        public string Description { get; set; }
        public int TicketId { get; set; }
    }
}