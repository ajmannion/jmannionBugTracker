using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jmannionBugTracker.Models;
using System.Web.Security;

namespace jmannionBugTracker.Controllers
{
    public class AdminController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        UserRoleAssignHelper helper = new UserRoleAssignHelper();
        // GET: Admin
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            List<AdminUserViewModel> userList = new List<AdminUserViewModel>();
            foreach (var users in db.Users.ToList())
            {
                var userCollection = new AdminUserViewModel();
                userCollection.user = users;
                userCollection.role = helper.ListUserRoles(users.Id).ToList();
                userList.Add(userCollection);
            }
            return View(userList);
        }

        // Get Admin/SelectRole/5

        public ActionResult SelectRole(string id)
        {
            var user = db.Users.Find(id);
            var roleUser = new UserRoleViewModel();
            roleUser.Id = user.Id;
            roleUser.FirstName = user.Id;
            roleUser.LastName = user.LastName;
            roleUser.DisplayName = user.DisplayName;
            roleUser.SelectedRoles = helper.ListUserRoles(user.Id).ToArray();
            roleUser.UserRoles = new MultiSelectList(db.Roles,"Name", "Name", roleUser.SelectedRoles);

            return View(roleUser);

        }

        //Post selectroles/5
        [HttpPost]
        public ActionResult SelectRole (UserRoleViewModel model)
        {

            var user = db.Users.Find(model.Id);
            foreach (var rolemv in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(user.Id, rolemv);
            }

            foreach (var rolemv in model.SelectedRoles)
            {

                helper.AddUserToRole(user.Id, rolemv);
            }
            ViewBag.confim = "User's role has been sucessfully modified";
            return RedirectToAction("Index");
        }



       





    }
}