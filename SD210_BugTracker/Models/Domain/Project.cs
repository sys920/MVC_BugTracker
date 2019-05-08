using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.Domain
{
    public class Project
    {
        public virtual List<ApplicationUser> Users { get; set; }

        public Project ()
        {
            Users = new List<ApplicationUser>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Boolean Archive { get; set; }
        public virtual List<Ticket> Ticket { get; set; }
    }
}
