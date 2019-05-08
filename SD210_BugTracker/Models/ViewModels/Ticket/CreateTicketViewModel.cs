using SD210_BugTracker.Models.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SD210_BugTracker.Models.ViewModels
{
    public class CreateTicketViewModel
    {        
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }

        [Required]
        public int ProjectId { get; set; }       
        public List<ProjectIdName> ProjectIdNameList { get; set; }

        [Required]
        public int TicketTypeId { get; set; }
        public List<TicketType> TicketTypeNameList { get; set; }

        [Required]
        public int TicketPriorityId { get; set; }
        public List<TicketPriority> TicketPriorityNameList { get; set; }
        
        
    }
}