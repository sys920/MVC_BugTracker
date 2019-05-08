using SD210_BugTracker.Models.Domain;
using SD210_BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext DbContext;

        public ProjectHelper(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public List<Project> GetAllProject()
        {
            return DbContext.Projects.Where(p => p.Archive != true).ToList();
        }

        public List<Project> GetMyProjectListByUserId(string UserId)
        {
            return DbContext.Projects.Where(p => p.Archive != true).Where( p => p.Users.Any( u => u.Id == UserId)).ToList();
        }

        public Project GetProjectDetailByProjectId(int? PId)
        {
            return DbContext.Projects.Where(p => p.Archive != true).FirstOrDefault(p => p.Id == PId); 
        }

        public List<ApplicationUser> GetUserIncludedProject(int? PId)
        {
            return DbContext.Users.Where(p => p.Projects.Where(u => u.Archive != true).Any(u => u.Id == PId)).ToList();
        }


        public List<ApplicationUser> GetUsersExcludedProject(int? PId)
        {
            return DbContext.Users.Where(p => !p.Projects.Where(u => u.Archive != true).Any(u => u.Id == PId)).ToList();
        }

        public void UpdateProjectMember(string UserId, int? PId, string Require)
        {
           
            var User = DbContext.Users.FirstOrDefault(p => p.Id == UserId);

            var Project = DbContext.Projects.Where(p => p.Archive != true).FirstOrDefault(u => u.Id == PId);

            if (Require == "AddUser")
            {
                Project.Users.Add(User);
            }
            else if (Require == "DeleteUser")
            {
                Project.Users.Remove(User);
            }
            
            DbContext.SaveChanges();           
        }
    }
}