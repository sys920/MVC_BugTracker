using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.Domain
{
    public class TicketHistoryChangedDetail
    {
        public int Id { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public virtual TicketHistory TicketHistory { get; set; }
        public int TicketHistoryId { get; set; }
    }
}