using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SD210_BugTracker.Models.Domain;

namespace SD210_BugTracker.Models.ViewModels
{
    public class EditTicketViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }  
        
        public DateTime? Updated { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public List<ProjectIdName> ProjectNameList { get; set; }
        
        [Required]
        public int TicketTypeId { get; set; }
        public List<TicketType> TicketTypeNameList { get; set; }
       
        [Required]
        public int TicketPriorityId { get; set; }
        public List<TicketPriority> TicketPriorityNameList { get; set; }
       
        [Required]
        public int TicketStatusId { get; set; }
        public List<TicketStatus> TicketStatusNameList { get; set; }
      
        public string AssignedToId { get; set; }
        public ApplicationUser AssignedTo { get; set; }
        
        public List<DeveloperName> DeveloperNameList { get; set; }
    }
}