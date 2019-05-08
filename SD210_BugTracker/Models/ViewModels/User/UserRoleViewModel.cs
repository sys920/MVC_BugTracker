using Microsoft.AspNet.Identity.EntityFramework;
using SD210_BugTracker.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserNickName { get; set; }
        public string UserName { get; set; }       
        public List <Boolean> UserRoles { get; set; }
    }
}