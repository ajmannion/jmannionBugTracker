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

namespace jmannionBugTracker.Controllers
{
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketComments
        [Authorize]
        public ActionResult Index(int? TicketId)
        {
            if (User.IsInRole("Admin"))
            {
                var ticketComments = db.TicketComments.OrderBy( o => o.TicketId).ToList();
                return View(ticketComments);
            }
            else
            {
                int? tid = TicketId;
                var ticketComments = db.TicketComments.Where(t => t.TicketId == tid).ToList();
                return View(ticketComments);
            }
            
        }
    



        // GET: TicketComments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // GET: TicketComments/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,TicketId, UserId,body,UpdatedDate,CreatedDate")] int ticketId, TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                ticketComment.UserId = db.Users.Find(User.Identity.GetUserId()).Id;
                ticketComment.CreatedDate = DateTime.Now;
                ticketComment.TicketId = ticketId;       
                db.TicketComments.Add(ticketComment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new {id = ticketComment.TicketId});
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // GET: TicketComments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,body,UpdatedDate,CreatedDate")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComment).State = EntityState.Modified;
                ticketComment.UpdatedDate = DateTime.Now;
                db.TicketComments.Attach(ticketComment);
                db.Entry(ticketComment).Property("TicketId").IsModified = false;
                db.Entry(ticketComment).Property("Id").IsModified = false;
                db.Entry(ticketComment).Property("CreatedDate").IsModified = false;
                db.Entry(ticketComment).Property("UpdatedDate").IsModified = true;
                db.Entry(ticketComment).Property("body").IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index", "TicketsComments" , new { id = ticketComment.TicketId });
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // GET: TicketComments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComment ticketComment = db.TicketComments.Find(id);
            db.TicketComments.Remove(ticketComment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
