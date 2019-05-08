using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.Domain
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
        public string CreatedById { get; set; }

        public virtual ApplicationUser AssignedTo { get; set; }
        public string AssignedToId { get; set; }

        public virtual TicketType TicketType { get; set; }
        public int TicketTypeId { get; set; }
       
        public virtual TicketPriority TicketPriority { get; set; }
        public int TicketPriorityId { get; set; }

        public virtual TicketStatus TicketStatus { get; set; }
        public int TicketStatusId { get; set; }

        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }
               
        public virtual List<TicketAttachment> TicketAttachments { get; set; }

        public virtual List<TicketComment> TicketComments { get; set; }

        public virtual List<TicketHistory> TicketHistories { get; set; }

        public virtual List<TicketNotification> TicketNotifications { get; set; }
    }
}