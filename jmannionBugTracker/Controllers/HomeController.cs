using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jmannionBugTracker.Models;

namespace jmannionBugTracker.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard()

        { 
                var ticket = db.Tickets;
                ViewBag.projectcount = db.Projects.Count();
                ViewBag.ticketcount = db.Tickets.Count();
                ViewBag.resolvedcount = db.Tickets.Where(t => t.TicketStatusId == 4).Count();
                ViewBag.opencount = db.Tickets.Where(t => t.TicketStatusId != 4).Count();
                ViewBag.usercount = db.Users.Count();
                return View();
                      
        }
        public ActionResult widgets()
        {
            ViewBag.Message = "BugTracker Widgets";

            return View();
        }


    }
}