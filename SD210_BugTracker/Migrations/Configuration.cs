namespace SD210_BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SD210_BugTracker.Models;
    using SD210_BugTracker.Models.Domain;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SD210_BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SD210_BugTracker.Models.ApplicationDbContext";
            
        }

        protected override void Seed(SD210_BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //to avoid creating duplicate seed data.

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    //! Uncomment line below to start debugging the Seed() method
            //    System.Diagnostics.Debugger.Launch();
            //}


            //Add RoleManager        
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //Add UserManager 
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Adding roles at first 
            if (!context.Roles.Any(p => p.Name == "Admin")) 
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }

            if (!context.Roles.Any(p => p.Name == "Project Manager"))
            {
                var projectManager = new IdentityRole("Project Manager");
                roleManager.Create(projectManager);
            }

            if (!context.Roles.Any(p => p.Name == "Developer"))
            {
                var developer = new IdentityRole("Developer");
                roleManager.Create(developer);
            }

            if (!context.Roles.Any(p => p.Name == "Submitter"))
            {
                var submitter = new IdentityRole("Submitter");
                roleManager.Create(submitter);
            }

            //Adding admin account at first 
            ApplicationUser adminUser;

            if (!context.Users.Any(p => p.UserName == "admin@mybugtracker.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserNickName = "Boss";
                adminUser.UserName = "admin@mybugtracker.com";
                adminUser.Email = "admin@mybugtracker.com";
                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context.Users.FirstOrDefault(p => p.UserName == "admin@mybugtracker.com");
            }
            //Adding  adminUser is on the adminRole
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");              
            }
            
            //Adding Demo-admin account
            ApplicationUser DemoAdmin;
            if (!context.Users.Any(p => p.UserName == "DemoAdmin@demo.com"))
            {
                DemoAdmin = new ApplicationUser();
                DemoAdmin.UserNickName = "Demo-Admin";
                DemoAdmin.UserName = "DemoAdmin@demo.com";
                DemoAdmin.Email = "DemoAdmin@demo.com";
                userManager.Create(DemoAdmin, "Password-1");
            }
            else
            {
                DemoAdmin = context.Users.FirstOrDefault(p => p.UserName == "DemoAdmin@demo.com");
            }

            if (!userManager.IsInRole(DemoAdmin.Id, "Admin"))
            {
                userManager.AddToRole(DemoAdmin.Id, "Admin");
            }

            //Adding Demo-PM account
            ApplicationUser DemoPM;
            if (!context.Users.Any(p => p.UserName == "DemoPM@demo.com"))
            {
                DemoPM = new ApplicationUser();
                DemoPM.UserNickName = "Demo-PM";
                DemoPM.UserName = "DemoPM@demo.com";
                DemoPM.Email = "DemoPM@demo.com";
                userManager.Create(DemoPM, "Password-1");
            }
            else
            {
                DemoPM = context.Users.FirstOrDefault(p => p.UserName == "DemoPM@demo.com");
            }

            if (!userManager.IsInRole(DemoPM.Id, "Project Manager"))
            {
                userManager.AddToRole(DemoPM.Id, "Project Manager");
            }

            //Adding Demo-developer account
            ApplicationUser DemoDeveloper;
            if (!context.Users.Any(p => p.UserName == "DemoDeveloper@demo.com"))
            {
                DemoDeveloper = new ApplicationUser();
                DemoDeveloper.UserNickName = "Demo-Developer";
                DemoDeveloper.UserName = "DemoDeveloper@demo.com";
                DemoDeveloper.Email = "DemoDeveloper@demo.com";
                userManager.Create(DemoDeveloper, "Password-1");
            }
            else
            {
                DemoDeveloper = context.Users.FirstOrDefault(p => p.UserName == "DemoDeveloper@demo.com");
            }

            if (!userManager.IsInRole(DemoDeveloper.Id, "Developer"))
            {
                userManager.AddToRole(DemoDeveloper.Id, "Developer");
            }

            //Adding Demo-submitter account
            ApplicationUser DemoSumitter;
            if (!context.Users.Any(p => p.UserName == "DemoSubmitter@demo.com"))
            {
                DemoSumitter = new ApplicationUser();
                DemoSumitter.UserNickName = "Demo-Submitter";
                DemoSumitter.UserName = "DemoSubmitter@demo.com";
                DemoSumitter.Email = "DemoSubmitter@demo.com";
                userManager.Create(DemoSumitter, "Password-1");
            }
            else
            {
                DemoSumitter = context.Users.FirstOrDefault(p => p.UserName == "DemoSubmitter@demo.com");
            }

            if (!userManager.IsInRole(DemoSumitter.Id, "Submitter"))
            {
                userManager.AddToRole(DemoSumitter.Id, "Submitter");
            }



            //Add Ticket Types   
            if (!context.TicketTypes.Any(p => p.Name == "Bug"))
            {
                var ticketType = new TicketType();
                ticketType.Name = "Bug";
                context.TicketTypes.Add(ticketType);
            }
            if (!context.TicketTypes.Any(p => p.Name == "Feature"))
            {
                var ticketType = new TicketType();
                ticketType.Name = "Feature";
                context.TicketTypes.Add(ticketType);
            }
            if (!context.TicketTypes.Any(p => p.Name == "Database"))
            {
                var ticketType = new TicketType();
                ticketType.Name = "Database";
                context.TicketTypes.Add(ticketType);
            }
            if (!context.TicketTypes.Any(p => p.Name == "Support"))
            {
                var ticketType = new TicketType();
                ticketType.Name = "Support";
                context.TicketTypes.Add(ticketType);
            }

            //Add Ticket properties   
            if (!context.TicketPriorities.Any(p => p.Name == "Low"))
            {
                var ticketPriority = new TicketPriority();
                ticketPriority.Name = "Low";
                context.TicketPriorities.Add(ticketPriority);
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Medium"))
            {
                var ticketPriority = new TicketPriority();
                ticketPriority.Name = "Medium";
                context.TicketPriorities.Add(ticketPriority);
            }
            if (!context.TicketPriorities.Any(p => p.Name == "High"))
            {
                var ticketPriority = new TicketPriority();
                ticketPriority.Name = "High";
                context.TicketPriorities.Add(ticketPriority);
            }

            //Add Ticket statuses   
            if (!context.TicketStatuses.Any(p => p.Name == "Open"))
            {
                var ticketStatus = new TicketStatus();
                ticketStatus.Name = "Open";
                context.TicketStatuses.Add(ticketStatus);
            }
            if (!context.TicketStatuses.Any(p => p.Name == "Resolved"))
            {
                var ticketStatus = new TicketStatus();
                ticketStatus.Name = "Resolved";
                context.TicketStatuses.Add(ticketStatus);
            }
            if (!context.TicketStatuses.Any(p => p.Name == "Rejected"))
            {
                var ticketStatus = new TicketStatus();
                ticketStatus.Name = "Rejected";
                context.TicketStatuses.Add(ticketStatus);
            }


        }
    }
}
