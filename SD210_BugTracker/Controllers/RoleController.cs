using SD210_BugTracker.Models;
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
    public class RoleController : Controller
    {
        private ApplicationDbContext DbContext;
        private UserRoleHelper UserRoleHelper;       

        public RoleController ()
        {
            DbContext = new ApplicationDbContext();
            UserRoleHelper = new UserRoleHelper(DbContext);          
        }

        
        //Display users role List
        [Authorize(Roles = "Admin")]
        public ActionResult UserRoleList()
        {
            ViewBag.RoleNames = DbContext.Roles.ToList();

            var model = UserRoleHelper.GetUserRoleAll().Select(p => new UsersListViewModel
            {
                UserId = p.Id,
                UserNickName = p.UserNickName,
                UserName = p.UserName,
                UserRoles = p.Roles.ToList(),

            }).ToList();

            return View(model);
        }

        [HttpGet]
        //Display user role detail
        [Authorize(Roles = "Admin")]
        public ActionResult UserRoleDetail(string userId)
        {
            if (userId == null)
            {
                return RedirectToAction(nameof(RoleController.UserRoleList));
            }

            ViewBag.RoleNames = DbContext.Roles.ToList();

            var result = UserRoleHelper.GetUserRolebyUserId(userId);

            if (result == null)
            {
                return RedirectToAction(nameof(RoleController.UserRoleList));
            }

            var model = new UserRoleViewModel();
            var userRoles = new List<Boolean>();

            model.UserId = result.Id;
            model.UserNickName = result.UserNickName;
            model.UserName = result.UserName;

            //Each User has Booleans List(as RoleNames) to mangae  
            foreach (var role in ViewBag.RoleNames)
            {
                if (result.Roles.Any(p => p.RoleId == role.Id))
                {
                    userRoles.Add(true);
                }
                else
                {
                    userRoles.Add(false);
                }
            }

            model.UserRoles = userRoles;

            return View(model);
        }

        //Update user role 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UserRoleDetail(UserRoleUpdateViewModel formData)
        {
            if (formData == null)
            {
                return RedirectToAction(nameof(RoleController.UserRoleDetail));
            }

            ViewBag.RoleNames = DbContext.Roles.ToList();

            for (var i = 0; i < ViewBag.RoleNames.Count; i++)
            {
                if (formData.UserRoles[i])
                {
                    //Add User Role                    
                    UserRoleHelper.AddUserRole(formData.UserId, ViewBag.RoleNames[i].Name);
                }
                else
                {   //Delete User Role 
                    UserRoleHelper.DeleteUserRole(formData.UserId, ViewBag.RoleNames[i].Name);
                }
            }
            return RedirectToAction("UserRoleList", "Role");
        }
    }
}