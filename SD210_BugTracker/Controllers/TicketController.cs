using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SD210_BugTracker.Models;
using SD210_BugTracker.Models.Domain;
using SD210_BugTracker.Models.Filters;
using SD210_BugTracker.Models.Helpers;
using SD210_BugTracker.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;         

namespace SD210_BugTracker.Controllers
{    
    [Authorize]
     public class TicketController : Controller
    {
        private ApplicationDbContext DbContext;
        private ProjectHelper ProjectHelper;
        private TicketHelper TicketHelper;
        private CustomEmailService CustomEmailService;

        public TicketController()
        {
            DbContext = new ApplicationDbContext();
            ProjectHelper = new ProjectHelper(DbContext);
            TicketHelper = new TicketHelper(DbContext);
            CustomEmailService = new CustomEmailService();           
        }

        //Create new Ticket [Get]
        [HttpGet]
        [Authorize(Roles = "Submitter")]
        public ActionResult CreateTicket()
        {
            var model = new CreateTicketViewModel();
            var userId = User.Identity.GetUserId();

            model.ProjectIdNameList = ProjectHelper.GetMyProjectListByUserId(userId).Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
            model.TicketTypeNameList = TicketHelper.GetTicketTypeNames();
            model.TicketPriorityNameList = TicketHelper.GetTicketPriorityNames();
            return View(model);
        }

        //Create new Ticket [Post]
        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult CreateTicket(CreateTicketViewModel formData)
        {
            var userId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                var model = new CreateTicketViewModel();

                model.ProjectIdNameList = ProjectHelper.GetMyProjectListByUserId(userId).Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
                model.TicketTypeNameList = TicketHelper.GetTicketTypeNames();
                model.TicketPriorityNameList = TicketHelper.GetTicketPriorityNames();
                return View(model);
            }

            var ticket = new Ticket();

            ticket.Title = formData.Title;
            ticket.Description = formData.Description;
            ticket.Created = DateTime.Now;
            ticket.CreatedById = userId;
            ticket.ProjectId = formData.ProjectId;
            ticket.TicketTypeId = formData.TicketTypeId;
            ticket.TicketPriorityId = formData.TicketPriorityId;

            //When new ticket is created, set status open(Id:1)
            ticket.TicketStatusId = 1;

            DbContext.Tickets.Add(ticket);
            DbContext.SaveChanges();

            return RedirectToAction(nameof(TicketController.OwnedTicketsList));
        }

        //Display All ticket List for admin and project manager    
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult TicketsList()
        {
            ViewBag.Title = "All Ticket(s)";
            var model = TicketHelper.GetAllTickets().Select(p => new TicketsListViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Project = p.Project.Name,
                Created = p.Created,
                Updated = p.Updated,
                TicketType = p.TicketType.Name,
                TicketStatus = p.TicketStatus.Name,
                TicketPriority = p.TicketPriority.Name,
                CreatedBy = p.CreatedBy.UserNickName,
                AssignedTo = p.AssignedTo?.UserNickName

            }).ToList();

            return View(model);
        }

        //Display My Ticket List
        public ActionResult MyTicketsList()
        {
            ViewBag.Title = "My Project's Ticket(s)";
            var userId = User.Identity.GetUserId();

            var model = (from T in DbContext.Tickets
                         where DbContext.Projects.Where(p => p.Archive != true).Where(p => p.Users.Any(q => q.Id == userId))
                               .Select(r => r.Id).Any(r => r == T.ProjectId)
                         select new TicketsListViewModel
                         {
                             Id = T.Id,
                             Title = T.Title,
                             Description = T.Description,
                             Project = T.Project.Name,
                             Created = T.Created,
                             Updated = T.Updated,
                             TicketType = T.TicketType.Name,
                             TicketStatus = T.TicketStatus.Name,
                             TicketPriority = T.TicketPriority.Name,
                             CreatedBy = T.CreatedBy.UserNickName,
                             AssignedTo = T.AssignedTo.UserNickName
                         }).ToList();

            return View("TicketsList", model);
        }

        //Display Assigned Ticket List 

        [Authorize(Roles = "Developer")]
        public ActionResult AssignedTicketsList()
        {
            ViewBag.Title = "Assigned Ticket(s)";
            var userId = User.Identity.GetUserId();
            var model = TicketHelper.GetTicketsByAssignedId(userId).Select(p => new TicketsListViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Project = p.Project.Name,
                Created = p.Created,
                Updated = p.Updated,
                TicketType = p.TicketType.Name,
                TicketStatus = p.TicketStatus.Name,
                TicketPriority = p.TicketPriority.Name,
                CreatedBy = p.CreatedBy.UserNickName,
                AssignedTo = p.AssignedTo?.UserNickName

            }).ToList();

            return View("TicketsList", model);
        }

        //Display Owned Ticket List 
        [Authorize(Roles = "Submitter")]
        public ActionResult OwnedTicketsList()
        {
            ViewBag.Title = "Owned Tickets";
            var userId = User.Identity.GetUserId();
            var model = TicketHelper.GetTicketsByOwnerId(userId).Select(p => new TicketsListViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Project = p.Project.Name,
                Created = p.Created,
                Updated = p.Updated,
                TicketType = p.TicketType.Name,
                TicketStatus = p.TicketStatus.Name,
                TicketPriority = p.TicketPriority.Name,
                CreatedBy = p.CreatedBy.UserNickName,
                AssignedTo = p.AssignedTo?.UserNickName

            }).ToList();

            return View("TicketsList", model);
        }

        //Edit Ticket 
        [HttpGet]
        public ActionResult EditTicket(int? tId)
        {
            var userId = User.Identity.GetUserId();

            if (tId == null)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            //User permission Check 
            var userHasTicket = false;
            userHasTicket = CheckUserHasTicket(tId, userId);
            if (userHasTicket != true)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            var result = TicketHelper.GetFirstTicketByTid(tId);
            var DeveloperRoleId = DbContext.Roles.FirstOrDefault(p => p.Name == "Developer").Id;

            if (result == null)
            {
                return RedirectToAction(nameof(TicketController.TicketsList));
            }

            var model = new EditTicketViewModel();

            model.Id = result.Id;
            model.Title = result.Title;
            model.Description = result.Description;
            model.ProjectId = result.ProjectId;
            model.TicketTypeId = result.TicketTypeId;
            model.TicketPriorityId = result.TicketPriorityId;
            model.TicketStatusId = result.TicketStatusId;
            model.AssignedToId = result.AssignedToId;

            model.AssignedTo = result.AssignedTo;

            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                model.ProjectNameList = ProjectHelper.GetAllProject().Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
            }
            else
            {
                model.ProjectNameList = ProjectHelper.GetMyProjectListByUserId(userId).Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
            }

            model.TicketTypeNameList = TicketHelper.GetTicketTypeNames();
            model.TicketPriorityNameList = TicketHelper.GetTicketPriorityNames();
            model.TicketStatusNameList = TicketHelper.GetTicketStatusNames();
            model.DeveloperNameList = TicketHelper.GetDeveloperNamesByRoleId(DeveloperRoleId);

            return View(model);
        }

        //Edit Ticket 
        [HttpPost]
        [LogActionFilter]
        public ActionResult EditTicket(EditTicketViewModel formData, int tId)
        {
            
            var userId = User.Identity.GetUserId();  
            var result = TicketHelper.GetFirstTicketByTid(tId);

            if (!ModelState.IsValid)
            {
                var DeveloperRoleId = DbContext.Roles.FirstOrDefault(p => p.Name == "Developer").Id;
                var model = new EditTicketViewModel();
                model.Id = result.Id;
                model.Title = result.Title;
                model.Description = result.Description;
                model.ProjectId = result.ProjectId;
                model.TicketTypeId = result.TicketTypeId;
                model.TicketPriorityId = result.TicketPriorityId;
                model.TicketStatusId = result.TicketStatusId;
                model.AssignedToId = result.AssignedToId;
                model.ProjectNameList = ProjectHelper.GetMyProjectListByUserId(userId).Select(p => new ProjectIdName { Id = p.Id, Name = p.Name }).ToList();
                model.TicketTypeNameList = TicketHelper.GetTicketTypeNames();
                model.TicketPriorityNameList = TicketHelper.GetTicketPriorityNames();
                model.TicketStatusNameList = TicketHelper.GetTicketStatusNames();
                model.DeveloperNameList = TicketHelper.GetDeveloperNamesByRoleId(DeveloperRoleId);

                return View(model);
            }

            //User permission Check 
            var userHasTicket = false;
            userHasTicket = CheckUserHasTicket(tId, userId);
            if (userHasTicket != true)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            //Notification text message 
            var concatChangedText = "";
            var ticketHistory = new TicketHistory();

            ticketHistory.UserId = User.Identity.GetUserId();
            ticketHistory.TicketId = tId;
            ticketHistory.Changed = DateTime.Now;

            DbContext.TicketHistories.Add(ticketHistory);
            DbContext.SaveChanges();

            result.Title = formData.Title;
            result.Description = formData.Description;
            result.ProjectId = formData.ProjectId;
            result.TicketTypeId = formData.TicketTypeId;
            result.TicketPriorityId = formData.TicketPriorityId;
            result.TicketStatusId = formData.TicketStatusId;
            result.AssignedToId = formData.AssignedToId;
            result.Updated = DateTime.Now;

            //Change Tracking in Entity Framework
            var oldValues = DbContext.Entry(result).OriginalValues;
            var newValues = DbContext.Entry(result).CurrentValues;   
          
            foreach (var entry in oldValues.PropertyNames)
            {
                if(!(entry == "Created" || entry == "Updated"))
                {
                    var  oldlValue = oldValues[entry]?.ToString();
                    var  newValue = newValues[entry]?.ToString();                                       

                    if (oldlValue != newValue)
                    {
                        DbContext.TicketHistoryChangedDetails.Add(new TicketHistoryChangedDetail
                        {
                            Property = entry,
                            OldValue = PropertyIdToName(entry, oldlValue),
                            NewValue = PropertyIdToName(entry, newValue),
                            TicketHistoryId = ticketHistory.Id
                        });

                        //Notification message 
                        concatChangedText = concatChangedText + " [" + entry + "]";

                        if (entry == "AssignedToId" && newValue != null)
                        {
                            SendingNotification(DbContext.Users.FirstOrDefault(p=>p.Id == newValue) , "new developer was assinged", concatChangedText);
                        }
                       
                    }
                }               
            }

            DbContext.SaveChanges();

            SendingNotificationForAdmin(tId, "changed", concatChangedText);

            //if the ticket is assigned to the developer, send email to developer but only updated by another user 
            if (result.AssignedTo != null)
            {
                if (result.AssignedTo.Email != User.Identity.GetUserName())
                {
                    SendingNotification(result.AssignedTo, "changed", concatChangedText);
                }
            }

            //Redirect view according to the user role
            if (User.IsInRole("Admin") || User.IsInRole("Project Mangaer"))
            {
                return RedirectToAction(nameof(TicketController.TicketsList));
            }
            else if (User.IsInRole("Developer"))
            {
                return RedirectToAction(nameof(TicketController.AssignedTicketsList));
            }
            else if (User.IsInRole("Submitter"))
            {
                return RedirectToAction(nameof(TicketController.OwnedTicketsList));
            }

            return RedirectToAction(nameof(TicketController.TicketsList));
        }

        //Get Property Name from Property ID
        public string PropertyIdToName(string property, string value)
        {               
            if(property == "ProjectId")
            {
                var valueToInt = Convert.ToInt32(value);
                return DbContext.Projects.FirstOrDefault( p=>p.Id == valueToInt).Name;
            }
            else if(property == "TicketTypeId")
            {
                var valueToInt = Convert.ToInt32(value);
                return DbContext.TicketTypes.FirstOrDefault(p => p.Id == valueToInt).Name;
            }
            else if (property == "TicketPriorityId")
            {
                var valueToInt = Convert.ToInt32(value);
                return DbContext.TicketPriorities.FirstOrDefault(p => p.Id == valueToInt).Name;
            }
            else if (property == "TicketStatusId")
            {
                var valueToInt = Convert.ToInt32(value);
                return DbContext.TicketStatuses.FirstOrDefault(p => p.Id == valueToInt).Name;
            }
            else if (property == "AssignedToId")
            {                
                if (DbContext.Users.FirstOrDefault(p => p.Id == value) !=null)
                {
                    return DbContext.Users.FirstOrDefault(p => p.Id == value).UserNickName; 
                }

                return "none";

            }
            
            return value;
        }

        //Display Detail of Ticket
        [HttpGet]
        public ActionResult DetailTicket(int? tId)

        {
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;

            if (tId == null)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            //User permission Check  
            var UserHasTicket = false;
            UserHasTicket = CheckUserHasTicket(tId, userId);
            if (UserHasTicket != true)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            var result = TicketHelper.GetFirstTicketByTid(tId);
            var model = new DetailTicketViewModel();

            model.Id = result.Id;
            model.Title = result.Title;
            model.Description = result.Description;
            model.Project = result.Project.Name;
            model.Created = result.Created;
            model.Updated = result.Updated;

            model.TicketType = result.TicketType.Name;
            model.TicketPriority = result.TicketPriority.Name;
            model.TicketStatus = result.TicketStatus.Name;
            
            model.CreatedBy = result.CreatedBy.UserNickName;

            if (result.AssignedTo == null)
            {
                model.AssignedTo = null;
            }
            else
            {
                model.AssignedTo = result.AssignedTo.UserNickName;
            }

            model.TicketAttachments = DbContext.TicketAttachments.Where(p => p.TicketId == result.Id).ToList();
            model.TicketCommentsList = DbContext.TicketComments.Where(u => u.TicketId == tId).Select(p => new TicketCommentHasUserNickname
            {
                Id = p.Id,
                Comment = p.Comment,
                Created = p.Created,
                UserNickName = p.User.UserNickName,
                UserId = p.UserId
            }).ToList();

            model.TicketHistoryList = DbContext.TicketHistories.Where(u => u.TicketId == tId).ToList();

            //Check admin and project manager are in the notification Table
            var IsNotificationActive = DbContext.TicketNotifications.Where(p => p.TicketId == tId).Any(u => u.UserId == userId);

            model.NotificationStaus = IsNotificationActive;

            return View(model);
        }

        //Upload file for ticket
        [HttpPost]
        public ActionResult UploadFile(DetailTicketViewModel formData)
        {
            if (formData.File == null)
            {
                return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.Id });
            }

            var userId = User.Identity.GetUserId();

            //User permission Check 
            var UserHasTicket = false;
            UserHasTicket = CheckUserHasTicket(formData.Id, userId);
            if (UserHasTicket != true)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            var attachment = new TicketAttachment();

            //File extension check 
            string fileExtension;
            if (formData.File != null)
            {
                fileExtension = Path.GetExtension(formData.File.FileName).ToLower();

                if (!Constants.AllowedFileExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("FileExtensionError", "Only.jpg, .jpeg, .png, .docx, .pdf, pptx, xlsx extensions are allowed.");
                    return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.Id });
                }

                var fileSize = formData.File.ContentLength;
                if (fileSize > 5120000)
                {
                    ModelState.AddModelError("", "The file size is limited under 2MB");
                    return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.Id });
                }
            }

            //File upload 
            if (formData.File != null)
            {
                if (!Directory.Exists(Constants.MappedUploadFolder))
                {
                    Directory.CreateDirectory(Constants.MappedUploadFolder);
                }

                var fileName = formData.File.FileName;
                var fullPathFileName = Constants.MappedUploadFolder + fileName;

                attachment.FileUrl = Constants.UploadFolder + fileName;
                attachment.FilePath = fileName;
                attachment.Description = fileName;
                attachment.Created = DateTime.Now;
                attachment.TicketId = formData.Id;
                attachment.UserId = User.Identity.GetUserId();
                formData.File.SaveAs(fullPathFileName);
            }

            DbContext.TicketAttachments.Add(attachment);
            DbContext.SaveChanges();

            //Notification for developer only file has added  by another user            
            var user = DbContext.Tickets.FirstOrDefault(p => p.Id == formData.Id);
            if (user.AssignedToId != null)
            {        
                if (User.Identity.GetUserName() != user.AssignedTo.Email)
                {
                    SendingNotification(user.AssignedTo, "a new file", formData.File.FileName);                    
                }               
            }

            SendingNotificationForAdmin(formData.Id, "a new file", formData.File.FileName);

            return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.Id });
        }

        //Deletefile attatchment for ticket
        [HttpPost]
        public ActionResult DeleteFile(DeleteFileUploadedViewModel formData)
        {
            var userId = User.Identity.GetUserId();

            //User permission Check 
            var userHasAttatchment = false;
            userHasAttatchment = CheckUserHasAttatchment(formData.Id, userId);
            if (userHasAttatchment != true)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            var attatchment = DbContext.TicketAttachments.FirstOrDefault(p => p.Id == formData.Id);
          
            //Delete file in upload folder 
            if (attatchment.FileUrl != null)
            {
                var fullPath = Server.MapPath(attatchment.FileUrl);
                System.IO.File.Delete(fullPath);               
            }

            DbContext.TicketAttachments.Remove(attatchment);
            DbContext.SaveChanges();

            //Notification for developer only file has added  by another user            
            var getDeveloper = DbContext.Tickets.FirstOrDefault(p => p.Id == formData.Id);
            if (getDeveloper != null)
            {
                if (User.Identity.GetUserName() != getDeveloper.AssignedTo.Email)
                {
                    SendingNotification(getDeveloper.AssignedTo, "a delete file", formData.FilePath);                   
                }
            }

            SendingNotificationForAdmin(formData.TicketId, "a delete file", formData.FilePath);

            return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.TicketId });
        }

        //Create ticketComment
        [HttpPost]
        public ActionResult CreateTicketComment(DetailTicketViewModel formData)
        {
            //User Permission check 
            var userId = User.Identity.GetUserId();
            if (!CheckUserHasTicket(formData.Id, userId))
            {
                return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.Id });
            }

            if (formData.Comment == null)
            {
                return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.Id });
            }

            var comment = new TicketComment();

            comment.Created = DateTime.Now;
            comment.TicketId = formData.Id;
            comment.UserId = User.Identity.GetUserId();
            comment.Comment = formData.Comment;

            DbContext.TicketComments.Add(comment);
            DbContext.SaveChanges();

            //Notification for developer only comment has added  by another user            
            var getDeveloper = DbContext.Tickets.FirstOrDefault(p => p.Id == formData.Id);
            if (getDeveloper.AssignedToId != null)
            {  
                if (User.Identity.GetUserName() != getDeveloper.AssignedTo.Email)
                {
                    SendingNotification(getDeveloper.AssignedTo, "a new comment", formData.Comment);                  
                }
            }

            SendingNotificationForAdmin(formData.Id, "a new comment", formData.Comment);

            return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.Id });
        }

        //Edit ticketComment
        [HttpPost]
        public ActionResult EditTicketComment(EditCommentViewModel formData)
        {
            //User permission Check for comment
            var userHasComment = false;
            var userId = User.Identity.GetUserId();

            userHasComment = CheckUserHasComment(formData.CID, userId);
            if (userHasComment != true)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            if (formData.Comment == null)
            {
                return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.TId });
            }

            var comment = DbContext.TicketComments.FirstOrDefault( p => p.Id == formData.CID);

            comment.Comment = formData.Comment;
            DbContext.SaveChanges();

            //Notification for developer only comment has added  by another user            
            var getDeveloper = DbContext.Tickets.FirstOrDefault(p => p.Id == formData.TId);
            if (getDeveloper.AssignedToId != null)
            {
                if (User.Identity.GetUserName() != getDeveloper.AssignedTo.Email)
                {
                    SendingNotification(getDeveloper.AssignedTo, "a edited comment", formData.Comment);                  
                }
            }

            SendingNotificationForAdmin(formData.TId, "a edited comment", formData.Comment);

            return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.TId });
        }

        //Delete ticketComment
        [HttpPost]
        public ActionResult DeleteTicketComment(EditCommentViewModel formData)
        {
            var userId = User.Identity.GetUserId();

            //User permission Check for comment
            var UserHasComment = false;

            UserHasComment = CheckUserHasComment(formData.CID, userId);
            if (UserHasComment != true)
            {
                return RedirectToAction(nameof(TicketController.MyTicketsList));
            }

            var comment = DbContext.TicketComments.FirstOrDefault(p => p.Id == formData.CID);

            DbContext.TicketComments.Remove(comment);
            DbContext.SaveChanges();

            //Notification for developer only comment has added  by another user            
            var getDeveloper = DbContext.Tickets.FirstOrDefault(p => p.Id == formData.TId);
            if (getDeveloper.AssignedToId != null)
            {
                if (User.Identity.GetUserName() != getDeveloper.AssignedTo.Email)
                {
                    SendingNotification(getDeveloper.AssignedTo, "a deleted comment", formData.Comment);                   
                }
            }

            SendingNotificationForAdmin(formData.TId, "a deleted comment", formData.Comment);
            return RedirectToAction("DetailTicket", "Ticket", new { tId = formData.TId });
        }
         
        //Check valid ticketId that user has it
        public bool CheckUserHasTicket(int? tId, string userId)
        {
            var result = false;
            var Ticket = TicketHelper.GetFirstTicketByTid(tId);

            if (Ticket == null)
            {
                return false;
            }

            if (User.IsInRole("Developer"))
            {
                if (Ticket.AssignedToId == userId)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (User.IsInRole("Submitter"))
            {
                if (Ticket.CreatedById == userId)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                result = true;
            }
            return result;
        }

        //Check valid CommentId that user has it
        public bool CheckUserHasComment(int? cId, string userId)
        {
            var result = false;
            var comment = TicketHelper.GetCommentByCId(cId);

            if (comment == null)
            {
                return false;
            }

            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                result = true;
            }
            else 
            {
                if (comment.UserId == userId)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            
            return result;
        }

        public bool CheckUserHasAttatchment(int? FId, string userId)
        {
            var result = false;
            var attachment = TicketHelper.GetAttatchmentByFId(FId);

            if (attachment == null)
            {
                return false;
            }

            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                result = true;
            }
            else
            {
                if (attachment.UserId == userId)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public void SendingNotification(ApplicationUser assignedTo, string keyword, string concatText)
        {
            MailAddress userEmailAddress = new MailAddress(assignedTo.Email, assignedTo.UserNickName);

            var message = new MailMessage();
            message.From = new MailAddress("system@mailtrap.io", "MailSystem");
            message.To.Add(userEmailAddress);
            message.Subject = "Ticket has " + keyword;
            message.Body = "The change(s) that " + concatText;

            CustomEmailService.Sending(message);
        }
               
        public void SendingNotificationForAdmin(int tId, string keyword, string concatText)
        {
            var SendEmilList = DbContext.TicketNotifications.Where(p => p.TicketId == tId).Select(u => new { u.User.Email, u.User.UserNickName }).ToList();

           if(SendEmilList.Count()!= 0)
            {
                var message = new MailMessage();
                message.From = new MailAddress("system@mailtrap.io", "MailSystem");
                foreach (var ele in SendEmilList)
                {
                    MailAddress userEmailAddress = new MailAddress(ele.Email, ele.UserNickName);
                    message.To.Add(userEmailAddress);
                }
                message.Subject = "Ticket has " + keyword;
                message.Body = "The change(s) that " + concatText + "(This email for only admin and porject Manager)";

                CustomEmailService.Sending(message);
            }            
        }

        public ActionResult ManageNotification(int tId, bool NotificationStaus)
        {    
            var userId = User.Identity.GetUserId();

            if (NotificationStaus)
            {
                var ticketNotification = new TicketNotification();
                ticketNotification.UserId = userId;
                ticketNotification.TicketId = tId;
                DbContext.TicketNotifications.Add(ticketNotification);
            }
            else
            {
                var ticketNotification = DbContext.TicketNotifications.Where(p => p.TicketId == tId).Where(p => p.UserId == userId).First();
                DbContext.TicketNotifications.Remove(ticketNotification);
            }

            DbContext.SaveChanges();

            return RedirectToAction("DetailTicket", "Ticket", new { tId = tId });
        }
    }
}