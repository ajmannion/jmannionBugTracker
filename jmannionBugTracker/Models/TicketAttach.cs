using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jmannionBugTracker.Models
{
    public class TicketAttach
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public string FilePath { get; set; }

        public string FileUrl { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserId { set; get; }

      

        public virtual Ticket Ticket { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}