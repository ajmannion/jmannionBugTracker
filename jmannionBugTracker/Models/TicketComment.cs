using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jmannionBugTracker.Models
{
    public class TicketComment
    {

        public int Id { get; set; }

        public int TicketId { get; set; }

        public string UserId { get; set; }

        [AllowHtml]
        public string body { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime CreatedDate { get; set; }


        

        public virtual Ticket Ticket { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}