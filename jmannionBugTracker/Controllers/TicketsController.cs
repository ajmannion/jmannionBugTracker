using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jmannionBugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Threading.Tasks;

namespace jmannionBugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ProjectAssignHelper helper = new ProjectAssignHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var projects = helper.ListUserProjects(userId);
            var tickets = new List<Ticket>();
            if (User.IsInRole("Admin"))
            {

                return View(db.Tickets.ToList());

            }

            else if (User.IsInRole("Project Manager"))
            {
                foreach (var p in projects)
                {
                    foreach (var t in p.Tickets)
                    {
                        tickets.Add(t);
                    }
                }
                return View(tickets);
            }
            else if (User.IsInRole("Developer"))
            {

                return View(db.Tickets.Where(t => t.AssignedToUserId == userId).ToList());

            }
            else if (User.IsInRole("Submitter"))
            {

                return View(db.Tickets.Where(t => t.OwnerUserId == userId).ToList());

            }
            return View(tickets);
        }



        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin,Project Manager,Submitter,Developer")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {

            var roles = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            var user = db.Users.Where(u => u.Roles.Any(r => r.RoleId == roles.Id));
            var currentUser = db.Users.Find(User.Identity.GetUserId());


            var project = currentUser.Projects.ToList();
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(project, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "Id,Title,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTime.Now;
                var currentUser = db.Users.Find(User.Identity.GetUserId());
                ticket.OwnerUser = currentUser;
                //ticket.TicketStatusId = 1;
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(x => x.Name == "Unassigned").Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();

            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Project Manager,Submitter,Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {

                HistoryHelper historyHelper = new HistoryHelper();

                StringBuilder updateMessage = new StringBuilder();

                var oldTicketInfo = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                if (oldTicketInfo.Title != ticket.Title)
                {
                    historyHelper.AddHistory(ticket.Id, "Title", oldTicketInfo.Title, ticket.Title, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Ticket Title: {0}, ", ticket.Title);
                }
                if (oldTicketInfo.TicketTypeId != ticket.TicketTypeId)
                {
                    var oldTicketType = db.TicketTypes.Find(oldTicketInfo.TicketTypeId).Name;
                    var newTicketType = db.TicketTypes.Find(ticket.TicketTypeId).Name;
                    historyHelper.AddHistory(ticket.Id, "Ticket Type", oldTicketType, newTicketType, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Ticket Type: {0}, ", ticket.TicketTypeId);
                }
                if (oldTicketInfo.TicketPriorityId != ticket.TicketPriorityId)
                {
                    var oldTicketPriority = db.TicketPriorities.Find(oldTicketInfo.TicketPriorityId).Name;
                    var newTicketPriority = db.TicketPriorities.Find(ticket.TicketPriorityId).Name;
                    historyHelper.AddHistory(ticket.Id, "Ticket Type", oldTicketPriority, newTicketPriority, User.Identity.GetUserId());
                    updateMessage.AppendFormat("Ticket Priority: {0}, ", ticket.TicketTypeId);
                }

                ticket.Updated = DateTime.Now;
                db.SaveChanges();
                // Add Notifcations 

                var developer = db.Users.Find(ticket.AssignedToUserId);
                if (developer != null && developer.Email != null)
                {
                    var svc = new EmailService();
                    var msg = new IdentityMessage();
                    msg.Destination = developer.Email;
                    msg.Subject = " Aj Bug Tracker Update:" + ticket.Title;
                    msg.Body = ("The following items have been updated  the :" + ticket.Title + updateMessage);
                    await svc.SendAsync(msg);

                }

                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);

        }

        //// POST: Tickets/Delete/5
        //[Authorize(Roles = "Admin,Project Manager")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Ticket ticket = db.Tickets.Find(id);
        //    db.Tickets.Remove(ticket);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //Mark the ticket closed with a "soft delete"
        [Authorize(Roles = "Admin,Project Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            var strLoginUser = User.Identity.GetUserId();
            HistoryHelper audit = new HistoryHelper();
            audit.AddHistory(ticket.Id, "Status", ticket.TicketStatus.Name, "Resolved", strLoginUser);
            ticket.TicketStatusId = 4;

            // Sends a notification
            var developer = db.Users.Find(ticket.AssignedToUserId);
            if (developer != null && developer.Email != null)
            {
                var svc = new EmailService();
                var msg = new IdentityMessage();
                msg.Destination = developer.Email;
                msg.Subject = " Aj's Bug Tracker Update: " + ticket.Title;
                msg.Body = ("Ticket ID: " + ticket.Id + " - " + ticket.Title + "has been resolved");
                await svc.SendAsync(msg);
            }

            db.Tickets.Attach(ticket);
            db.Entry(ticket).Property("TicketStatusId").IsModified = true;
            db.SaveChanges();

            return RedirectToAction("Index", new { Id = ticket.Id });

}
    
        // GET: Tickets/Assign
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult AssignTicket(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket tickets = db.Tickets.Find(id);
               if (tickets == null)
            {
                return HttpNotFound();
            }

            var roles = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            var user = db.Users.Where(u => u.Roles.Any(r => r.RoleId == roles.Id));
          

            ViewBag.AssignedToUserId = new SelectList(user, "Id", "FirstName",tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName",tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name",tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name",tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name",tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }

        // POST: Tickets/Assign
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult AssignTicket([Bind(Include = "Id,Title,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Updated = DateTime.Now;
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(x => x.Name == "Assigned").Id;
                // db.Entry(ticket).State = EntityState.Modified;
                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property("TicketTypeId").IsModified = false;
                db.Entry(ticket).Property("ProjectId").IsModified = false;
                db.Entry(ticket).Property("TicketPriorityId").IsModified = false;
                db.Entry(ticket).Property("TicketStatusId").IsModified = true;
                db.Entry(ticket).Property("OwnerUserId").IsModified = false;
                db.Entry(ticket).Property("AssignedToUserId").IsModified = true;
                db.Entry(ticket).Property("Id").IsModified = false;
                db.Entry(ticket).Property("Created").IsModified = false;
                db.Entry(ticket).Property("Title").IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
