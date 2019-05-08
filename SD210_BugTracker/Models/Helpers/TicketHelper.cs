using SD210_BugTracker.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.Entity;
using Microsoft.AspNet.Identity;
using SD210_BugTracker.Models;

using SD210_BugTracker.Models.Helpers;
using SD210_BugTracker.Models.ViewModels;


using System.IO;


using System.Web.Mvc;


namespace SD210_BugTracker.Models.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext DbContext;

        public  TicketHelper(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;             
        }

        public List<Ticket> GetAllTickets()
        {
            return DbContext.Tickets.Where(p => p.Project.Archive != true).ToList();
        }

        public List<Ticket> GetTicketsByAssignedId(string userId)
        {
            return DbContext.Tickets.Where(p => p.AssignedToId == userId).Where( p => p.Project.Archive != true).ToList();
        }

        public List<Ticket> GetTicketsByOwnerId(string userId)
        {
            return DbContext.Tickets.Where(p => p.CreatedById == userId).Where(p => p.Project.Archive != true).ToList();
        }

        public List<TicketType> GetTicketTypeNames()
        {
            return DbContext.TicketTypes.ToList();
        }

        public List<TicketPriority> GetTicketPriorityNames()
        {
            return DbContext.TicketPriorities.ToList();
        }

        public List<TicketStatus> GetTicketStatusNames()
        {
            return DbContext.TicketStatuses.ToList();
        }
                
        public Ticket GetFirstTicketByTid(int? tId)
        {
            var result = DbContext.Tickets.Where(p => p.Id == tId).Where(p => p.Project.Archive != true);
            if (result.Count() == 0)
            {
                return null;
            }
            return result.First();
        }

        public TicketComment GetCommentByCId(int? cId)
        {
            var result = DbContext.TicketComments.FirstOrDefault(p => p.Id == cId);
           
            return result;
        }

        public TicketAttachment GetAttatchmentByFId(int? fId)
        {
            var result = DbContext.TicketAttachments.FirstOrDefault(p => p.Id == fId);

            return result;
        }

        public List<DeveloperName> GetDeveloperNamesByRoleId(string developerRoleId)
        {
           return DbContext.Users.Where(p => p.Roles.Any(u => u.RoleId == developerRoleId)).Select(p => new DeveloperName { Id = p.Id, Name = p.UserNickName }).ToList();
        }

    }
}