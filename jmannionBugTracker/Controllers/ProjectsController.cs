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
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ProjectAssignHelper helper = new ProjectAssignHelper();

        // GET: Projects
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult Index()
        {
            var users = helper.ListProjects(User.Identity.GetUserId());
            return View(users);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var role = db.Roles.FirstOrDefault(r => r.Name == "Project Manager");
            var users = db.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id)).ToList();
            ViewBag.OwnerName = new SelectList(users, "Id", "LastName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles ="Admin,Project Manager")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Owner,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Owner,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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

       // Get Admin/SelectRole/5
         [Authorize(Roles = "Admin,Project Manager")]

        public ActionResult AssignToProject(int id)
        {
            var project = db.Projects.Find(id);
            var ProjectUser = new AssignToProjectVM();
            ProjectUser.Id = id;
            ProjectUser.FirstName = ProjectUser.FirstName;
            ProjectUser.Lastname = ProjectUser.Lastname;
            ProjectUser.SelectedUsers = helper.ListUserOnProject(project.Id).Select(p => p.Id).ToArray();
            ProjectUser.userList = new MultiSelectList(db.Users, "Id", "Firstname", ProjectUser.SelectedUsers);

             return View(ProjectUser);

        }

        //Post selectroles/5
        [HttpPost]
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult AssignToProject(AssignToProjectVM model)
        {

            foreach (var userRmv in db.Users.Select(r => r.Id).ToList())
            {
                helper.RemoveUserFromProject(userRmv, model.Id);
            }

            foreach (var userTmv in model.SelectedUsers)
            {

                helper.AddUserToProject(userTmv, model.Id);
            }
           // ViewBag.confim = "Project has been sucessfully modified";
            return RedirectToAction("Details", "Projects", new { id = model.Id });
        }










    }

}
