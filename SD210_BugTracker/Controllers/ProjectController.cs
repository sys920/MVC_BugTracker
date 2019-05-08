using Microsoft.AspNet.Identity;
using SD210_BugTracker.Models;
using SD210_BugTracker.Models.Domain;
using SD210_BugTracker.Models.Helpers;
using SD210_BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SD210_BugTracker.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private ApplicationDbContext DbContext;
        private ProjectHelper ProjectHelper;
       
        public ProjectController ()
        {
            DbContext = new ApplicationDbContext();
            ProjectHelper = new ProjectHelper(DbContext);
        }

        //Create new projects 
        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult CreateProject(CreateProjectViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var project = new Project();

            project.Name = formData.Name;
            project.Description = formData.Description;
            project.CreateDate = DateTime.Now;

            DbContext.Projects.Add(project);
            DbContext.SaveChanges();

            return RedirectToAction("ProjectsList", "Project");
        }

        //Display All Projects List
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ProjectsList()
        {
            ViewBag.Title = "All Projects";          

            var model = ProjectHelper.GetAllProject().Select(p => new ProjectsListViewModel
            {

                Id = p.Id,
                Name = p.Name,
                NumberOfMembers = ProjectHelper.GetUserIncludedProject(p.Id).Count(),
                NumberOfTickets = DbContext.Tickets.Where(u => u.ProjectId == p.Id).ToList().Count(),
                CreateDate = p.CreateDate,
                UpdateDate = p.UpdateDate,

            }).ToList();

            return View("ProjectsList",model);
        }

        [Authorize]
        //Display My Projects List 
        public ActionResult MyProjectsList()
        {
            ViewBag.Title = "My Projects";
           
            var userId = User.Identity.GetUserId();
            var model = ProjectHelper.GetMyProjectListByUserId(userId).Select(p => new ProjectsListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                NumberOfMembers = ProjectHelper.GetUserIncludedProject(p.Id).Count(),
                NumberOfTickets = DbContext.Tickets.Where(u => u.ProjectId == p.Id).ToList().Count(),
                CreateDate = p.CreateDate,
                UpdateDate = p.UpdateDate
            }).ToList();

            return View("ProjectsList", model);
        }

        //Display project detail 
        [Authorize]
        public ActionResult ProjectDetail(int? pId)
        {
            if (pId == null)
            {
                return RedirectToAction(nameof(ProjectController.MyProjectsList));
            }
            
            var result = ProjectHelper.GetProjectDetailByProjectId(pId);
           
            if (result == null)
            {
                return RedirectToAction(nameof(ProjectController.MyProjectsList));
            }           

            //Check user is belong to the project
            if (!(User.IsInRole("Admin") || User.IsInRole("Project Manager")))
            {
                var CheckUserHasProject = CheckUserHasProjectByProjectId(pId);

                if (CheckUserHasProject != true)
                {
                    return RedirectToAction(nameof(ProjectController.MyProjectsList));
                }
            }

            var model = new ProjectDetailViewModel();

            model.Id = result.Id;
            model.Name = result.Name;
            model.NumberOfMembers = ProjectHelper.GetUserIncludedProject(pId).Count();
            model.NumberOfTickets = DbContext.Tickets.Where(u => u.ProjectId == pId).ToList().Count();
            model.Description = result.Description;
            model.CreateDate = result.CreateDate;
            model.UpdateDate = result.UpdateDate;

            return View(model);
        }

        //Edit project detail
        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult EditProject(int pId)
        {
            var result = ProjectHelper.GetProjectDetailByProjectId(pId);

            if (result == null)
            {
                return RedirectToAction(nameof(ProjectController.ProjectsList));
            }

            var model = new CreateProjectViewModel();

            model.Id = result.Id;
            model.Name = result.Name;
            model.Description = result.Description;
            model.Archive = result.Archive;

            return View(model);
        }

        //Edit project detail post
        [HttpPost]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult EditProject(CreateProjectViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = ProjectHelper.GetProjectDetailByProjectId(formData.Id);

            if (result == null)
            {
                return View();
            }

            result.Name = formData.Name;
            result.Description = formData.Description;
            result.UpdateDate = DateTime.Now;
            result.Archive = formData.Archive;

            DbContext.SaveChanges();

            return RedirectToAction("ProjectDetail", "Project", new { PId = formData.Id });
        }

        //Display Project Members and User 
        [HttpGet]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ManageProjectMembers(int? PId, string UserId, string Require)
        {
            if (!PId.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.ProjectsList));
            }

            var result = ProjectHelper.GetProjectDetailByProjectId(PId);

            if (result == null)
            {
                return RedirectToAction(nameof(ProjectController.ProjectsList));
            }

            ViewBag.RoleNames = DbContext.Roles.ToList();
            ViewBag.PId = PId;
            ViewBag.ProjectName = result.Name;

            // Update project member when UserId and Require are 
            if (UserId != null && PId != null && Require != null)
            {
                ProjectHelper.UpdateProjectMember(UserId, PId, Require);
            }

            var users = ProjectHelper.GetUsersExcludedProject(PId);
            var members = ProjectHelper.GetUserIncludedProject(PId);

            var model = new ManageMembersViewModel();

            model.Users = users;
            model.Members = members;

            return View(model);
        }

        //Check this User is included in this project  
        public bool CheckUserHasProjectByProjectId(int? PId)
        {
            var userId = User.Identity.GetUserId();
            var CheckUserHasProject = false;
            var projectUser = DbContext.Users.Where(u => u.Id == userId).FirstOrDefault(p => p.Projects.Where(u => u.Archive != true).Any(u => u.Id == PId));

            if (projectUser != null)
            {
                CheckUserHasProject = true;
            }

            return CheckUserHasProject;
        }

    }
}