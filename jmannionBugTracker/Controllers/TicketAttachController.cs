using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jmannionBugTracker.Models;
using System.IO;

namespace jmannionBugTracker.Controllers
{
    public class TicketAttachController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttach
        public ActionResult Index()
        {
            var ticketAttaches = db.TicketAttachs.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttaches.ToList());
        }

        // GET: TicketAttach/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttach ticketAttach = db.TicketAttachs.Find(id);
            if (ticketAttach == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttach);
        }

        // GET: TicketAttach/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttach/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,FilePath,Description,CreatedDate,UserId")] TicketAttach ticketAttach, HttpPostedFileBase FileUrl, string description)
        {
            if (ModelState.IsValid)
            {

                if (FileUploadValidator.IsWebFriendlyFile(FileUrl))
                { 
                    var fileName = Path.GetFileName(FileUrl.FileName);
                    FileUrl.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), fileName));
                    ticketAttach.FileUrl = "~/Uploads/" + fileName;

                }


             
                ticketAttach.CreatedDate = DateTime.Now;
                ticketAttach.Description = description;
                db.TicketAttachs.Add(ticketAttach);
                db.SaveChanges();
                return RedirectToAction("Index");
         }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttach.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttach.UserId);
            return View(ticketAttach);
        }

        // GET: TicketAttach/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttach ticketAttach = db.TicketAttachs.Find(id);
            if (ticketAttach == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttach.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttach.UserId);
            return View(ticketAttach);
        }

        // POST: TicketAttach/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,FilePath,FileUrl,Description,CreatedDate,UserId")] TicketAttach ticketAttach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttach.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttach.UserId);
            return View(ticketAttach);
        }

        // GET: TicketAttach/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttach ticketAttach = db.TicketAttachs.Find(id);
            if (ticketAttach == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttach);
        }

        // POST: TicketAttach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttach ticketAttach = db.TicketAttachs.Find(id);
            db.TicketAttachs.Remove(ticketAttach);
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
