namespace jmannionBugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<jmannionBugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(jmannionBugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
               new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            var userManager = new UserManager<Models.ApplicationUser>(
                new UserStore<Models.ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "john.aj.mannion@gmail"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "john.aj.mannion@gmail.com",
                    Email = "john.aj.mannion@gmail.com",

                    DisplayName = "john.aj.mannion@gmail.com"
                }, "True78&*");
            }
            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",

                    DisplayName = "moderator@coderfoundry.com"
                }, "Password-1");
            }
            if (!context.Users.Any(u => u.Email == "vettel@gmail.com"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "vettel@gmail.com",
                    Email = "vettel@gmail.com",

                    DisplayName = "vettel@gmail.com"
                }, "Password-1");
            }

            if (!context.Users.Any(u => u.Email == "rosburg@gmail.com"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "rosburg@gmail.com",
                    Email = "rosburg@gmail.com",

                    DisplayName = "rosburg@gmail.com"
                }, "Password-1");
            }

            if (!context.Users.Any(u => u.Email == "DemoAdmin@gmail.com"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "DemoAdmin@gmail.com",
                    Email = "DemoAdmin@gmail.com",

                    DisplayName = "DemoAdmin@gmail.com"
                }, "Password-1");
            }
            var userId4 = userManager.FindByEmail("DemoAdmin@gmail.com").Id;
            userManager.AddToRole(userId4, "Admin");

            if (!context.Users.Any(u => u.Email == "DemoProjectManger@gmail.com"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "DemoProjectManager@gmail.com",
                    Email = "DemoProjectManager@gmail.com",

                    DisplayName = "DemoProjectManager@gmail.com"
                }, "Password-1");
            }
            var userId5 = userManager.FindByEmail("DemoProjectManager@gmail.com").Id;
            userManager.AddToRole(userId5, "Project Manager");
            if (!context.Users.Any(u => u.Email == "DemoDeveloper@gmail.com"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "DemoDeveloper@gmail.com",
                    Email = "DemoDeveloper@gmail.com",

                    DisplayName = "DemoDeveloper@gmail.com"
                }, "Password-1");
            }
            var userId6 = userManager.FindByEmail("DemoDeveloper@gmail.com").Id;
            userManager.AddToRole(userId6, "Developer");

            if (!context.Users.Any(u => u.Email == "DemoSubmitter@gmail.com"))
            {
                userManager.Create(new Models.ApplicationUser
                {
                    UserName = "DemoSubmitter@gmail.com",
                    Email = "DemoSubmitter@gmail.com",

                    DisplayName = "DemoSubmitter@gmail.com"
                }, "Password-1");
            }
            var userId7 = userManager.FindByEmail("DemoSubmitter@gmail.com").Id;
            userManager.AddToRole(userId7, "Submitter");





            var userId = userManager.FindByEmail("john.aj.mannion@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
            var user_Id = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(user_Id, "Project Manager");
            var user_Id1 = userManager.FindByEmail("vettel@gmail.com").Id;
            userManager.AddToRole(user_Id, "Developer");
            var user_Id2 = userManager.FindByEmail("rosburg@gmail.com").Id;
            userManager.AddToRole(user_Id, "Submitter");
        }
    }
}

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
 