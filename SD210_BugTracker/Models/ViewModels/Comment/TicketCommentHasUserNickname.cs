using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD210_BugTracker.Models.ViewModels
{
    public class TicketCommentHasUserNickname
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public string UserNickName { get; set; }
        public string UserId { get; set; }
    }
}