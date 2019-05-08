using SD210_BugTracker.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.ViewModels
{
    public class DetailTicketViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        [Required]
        public string Project { get; set; }
        [Required]
        public string TicketType { get; set; }
        [Required]
        public string TicketPriority { get; set; }
        public string TicketStatus { get; set; }

        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }

        public HttpPostedFileBase File { get; set; }
        public List<TicketAttachment> TicketAttachments { get; set; }

        public string Comment { get; set; }
        public List<TicketComment> TicketComments { get; set; }

        public List<TicketCommentHasUserNickname> TicketCommentsList { get; set; } 

        public List<TicketHistory> TicketHistoryList { get; set; }

        public bool NotificationStaus { get; set; }
    }

}