using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.ViewModels
{
    public class UserRoleUpdateViewModel
    {
        public string UserId { get; set; }
        public List<Boolean> UserRoles { get; set; }
    }
}