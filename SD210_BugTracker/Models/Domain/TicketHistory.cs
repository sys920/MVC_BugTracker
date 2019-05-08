using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.Domain
{
    public class TicketHistory
    {
        public int Id { get; set; }

        public DateTime  Changed { get; set; }

        public virtual Ticket Ticket { get; set; }
        public int TicketId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public virtual List<TicketHistoryChangedDetail> TicketHistoryChangedDetail { get; set; }       
    }
}