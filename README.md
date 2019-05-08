# C# MVC Bug Tracker 
A simple, fast and modern designed bug tracking application that helps you manage bugs easily and deliver great products on time. It is a web-based bug tracking and general purpose issue tracking application. You can login right now using demo account (Each roles)

- This application gives Role(Admin, Project Manager, Developer and Summiter) base member management 
- Each Project can have ticket that Summiter only create, Admin or Project Manager can edit and assing developer.
- Each Ticket has description , type, priority and status. Also comment and attachemet. 
- There are email-notification for Admin and Developer when ticket has edited or new comment and attatchment and Ticket history thatn all issue related the ticket. 

# Code
ASP.NET Web Application (.NET Framework 4.7) and using bootstrap template 'startbootstrap-sb-admin-2-gh-pages'

- This source code doesn't have Web.config, Startup.Auth.cs because of personal informaiton.

- Controller/AccountController : Add Demo Action for Creating Demo user (Admin, Project Manager, Developer, Summiter)
- Controller/HomeController : Only action for Dashboard (The number of project and ticket) each 
- Controller/ProjectController : Create, Edit, List and details about project 
- Controller/RoleController : Managing user role (Create, Edit) 
- Controller/TicketController : Create, Edit, List and details about Ticket include comment and attatchment.
- Migrations/Configuration.cs : First Seed data (Admin, Role, Demo user, Ticket properties)
- Models/Domain : project.cs, Ticket.cs, Ticket.cs, TicketAttatchment.cs, TicketCommnet.cs, TicketHistory.cs(one history has many chagesDetail),icketHistoryChangedDetail.cs(each change of ticket),TicketNotification.cs (Admin-List for receving notification)
- Models/Filters/AuthorizationFilter.cs : When user access any page that don't have permission, redirect to the error page. 
- Models/Filters/LogActionFilter.cs : Saving every action (ActionDescription and action name)to the data base 
- Models/Filters/LogExceptionFilter.cs : Redirect to the specific error page when internal error occurs
- Models/Helpers : Help Each conroller when it is using same linq 
- Models/ViewModels/Comment : for comment create, edit, List
- Models/ViewModels/Project : for Project create, edit, List
- Models/ViewModels/Ticket : for ticket create, edit, List
- Models/ViewModels/User : for user role updating
- Models/ViewModels/DashboardViewModel.cs : for dashboard page
- Models/ViewModels/ManageMembersViewModel.cs : for managing project member
- Models/ActionLog.cs : for action log 
- Models/CustomEmailService.cs : for custom eamil servie (using mail trap)
- Models/ExceptionLog.cs : for saving Exception log to the database
- Views/Account : Managing user account
- Views/Home/Dashboard.cshtml : Dashboard page (The numner of project and ticket)
- Views/Home/index.cshtml : for demo user login 
- Views/Home/unauthorized.cshtml : custom error page don't have permission
- Views/Project : Relate to the pages for project 
- Views/Role : Relate to the pages for Role
- Views/Ticket : Relate to the pages for Ticket 
- Views/shared/_Layout.cshtml : custom layout ( startbootstrap-sb-admin-2-gh-pages)
- Global.asax : Apply filters to the global 

# Requirement 
- Make sure the security is always added to the Controller. All actions that require authentication should be validated. 
- Administrators and Project Managers must be able to assign and unassigned users to and from projects. 
– Administrators, Project Managers, Developers, and Submitters must be able to view a list of projects they are assigned to. 
- Administrators and Project Managers must be able to view a separate list of all projects. This list should display the name of the project, how many users are assigned, how many tickets the project have, when it was created and when it was last updated. 

- Submitters only must be able to create tickets. The system should allow Submitters to create tickets only to the projects to which they are assigned. 

- Administrators and Project Managers must be able to view a list of all tickets belonging to all projects.
- Developers must be able to view a list of all tickets belonging to the projects to which they are assigned and also tickets they are assigned.
- Submitters must be able to view a list of all tickets belonging to the projects to which they are assigned and also their own tickets.
- Admin and Project Managers must be able to assign tickets to Developers only.
- Developers must be able to edit tickets to which they are assigned. 
- Submitters must be able to edit tickets they own
- A history must be generated for each property change made to a ticket (Project, Title, Description, Type, Status, Priority and Assigned Developer). 

- Administrators and Project Managers must be able to add, edit, delete Comments to any ticket.
- Developers must be able to add, edit, delete Comments to tickets to which they are assigned.
- Submitters must be able to add, edit, delete Comments to tickets they own.

- Administrators and Project Managers must be able to add, delete Attachments to any ticket.
- Developers must be able to add, delete Attachments to tickets to which they are assigned.
- Submitters must be able to add, delete Attachments to tickets they own.

- Developers must be notified by e-mail each time they are assigned to a ticket.
- Developers must be notified by e-mail each time a ticket to which they are assigned is modified by another user, including the addition of comments and attachments.(only when it is created)
- Admins and Project Managers can opt-in and opt-out to receive notifications by e-mail each time a ticket is modified, including the addition of comments and attachments. The application should allow the user to specify what tickets they would like to receive/cancel notifications from.

- Admins and Project Managers should be able to archive projects. Information from archived projects won’t    show anywhere in the system.
- A Landing page should be created with demo logins for each role.




