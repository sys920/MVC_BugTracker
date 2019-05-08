using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SD210_BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.Helpers
{
    public class UserRoleHelper
    {
        private ApplicationDbContext DbContext;
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;


        public UserRoleHelper(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));  
        }

        public List<ApplicationUser> GetUserRoleAll ()
        {
            return DbContext.Users.ToList();
        }

        public ApplicationUser GetUserRolebyUserId(string userId)

        {
            return DbContext.Users.FirstOrDefault(p => p.Id == userId);
        }

        public void AddUserRole(string userId, string roleName)

        {
            if (!UserManager.IsInRole(userId, roleName))
            {
                UserManager.AddToRole(userId, roleName);
            }

        }

        public void DeleteUserRole(string userId, string roleName)

        {
            if (UserManager.IsInRole(userId, roleName))
            {
                UserManager.RemoveFromRole(userId, roleName);
            }
        }
    }
}