using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jmannionBugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public String Name { get; set; } 

        public String Description { get; set; }

        public String Owner { get; set; }
        public Boolean Status { get; set; }
       
        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Users = new HashSet<ApplicationUser>();
        }
        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}