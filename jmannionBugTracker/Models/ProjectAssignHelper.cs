using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jmannionBugTracker.Models
{
    public class ProjectAssignHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserRoleAssignHelper userHelper = new UserRoleAssignHelper();
        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = project.Users.Any(u => u.Id == userId);
            return (user);
        }
        public void AddUserToProject(string userId, int projectId)
        {
            ApplicationUser user = db.Users.Find(userId);
            Project project = db.Projects.Find(projectId);
            project.Users.Add(user);
            db.SaveChanges();
        }
        public void RemoveUserFromProject(string UserId, int projectId)
        {
            ApplicationUser user = db.Users.Find(UserId);
            Project project = db.Projects.Find(projectId);
            project.Users.Remove(user);
            db.SaveChanges();
        }
         public List<Project> ListUserProjects(string userId )
        {
            ApplicationUser user = db.Users.Find(userId);
            return user.Projects.ToList();
        }
        public List<ApplicationUser> ListUserOnProject(int projectId)
        {
        Project project = db.Projects.Find(projectId);
        return project.Users.ToList();
        }
    public List<ApplicationUser> ListUsersNotOnProject (int projectId)
    {
            //Project  project= db.Projects.Find(projectId);
            //  var userIds = project.Users.Select(u => u.Id); ;
            // return db.Users.Where(u => !userIds.Contains(u.Id)).ToList();
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();

    }
        public List<Project> ListProjects(string user)

        {
            var users = db.Users.Find(user);

            var resultList = new List<Project>();
            var userProjects = new List<Project>();

            // Find user projects
            if (userHelper.IsUserInRole(users.Id, "Admin"))
            {
                userProjects = db.Projects.ToList();

            }
            else if (userHelper.IsUserInRole(users.Id, "ProjectManager"))//project manager list
            {
                var projOwnerId = db.Users.Find(users.Id);

                var projOwnerName = db.Users.FirstOrDefault(x => x.Id == projOwnerId.Id).DisplayName;

                userProjects = db.Projects.Where(p => p.Owner.Contains(projOwnerName)).ToList();
            }

            else
            {
                var UserId = db.Users.Find(users.Id);
                userProjects = UserId.Projects.ToList();

            }

            foreach (var project in userProjects)
            {
                // Only return projects not marked as complete
                Project projects = new Project();
                if (!project.Status)
                {
                    resultList.Add(project);
                }
            }

            return resultList;

        }


    }
}