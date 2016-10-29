﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace jmannionBugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public ApplicationUser()
        {
            this.AssignedTickets = new HashSet<Ticket>();

            this.TicketComments = new HashSet<TicketComment>();

            this.TicketHistories = new HashSet<TicketHistory>();

            this.TicketNotications = new HashSet<TicketNotification>();

            this.TicketAttachs = new HashSet<TicketAttach>();

            this.Projects = new HashSet<Project>();


        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<Ticket> AssignedTickets { get; set;}

        public virtual ICollection<TicketComment> TicketComments { get; set; }

        public virtual ICollection<TicketNotification> TicketNotications { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }

        public virtual ICollection<TicketAttach> TicketAttachs { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus>  TicketStatuses {get; set;}
        public DbSet<TicketAttach> TicketAttachs { get; set; }

        public DbSet<TicketComment> TicketComments { get; set; }

        public DbSet<TicketNotification> TicketNotifications { get; set; }

        public DbSet<TicketPriority > TicketPriorities { get; set; }

       public DbSet<TicketType> TicketTypes { get; set; }

        public DbSet<TicketHistory> TicketHistories { get; set; }
         public DbSet<Project>Projects { get; set; }






    }
}