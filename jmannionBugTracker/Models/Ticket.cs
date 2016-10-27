using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jmannionBugTracker.Models
{
    public class Ticket
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public int ProjectId {get; set;}

        public int TicketTypeId { get; set; }

        public int TicketPriorityId { get; set; }

        public int TicketStatusId { get; set; }

        public string OwnerUserId { get; set; }

        public string AssignedToUserId { get; set; }

        // Ticket Constructor

        public Ticket()
        {
            this.TicketAttachs = new HashSet<TicketAttach>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketNotifications = new HashSet<TicketNotification>();
            
        }

        // nav prop for for children

        public virtual ICollection<TicketAttach> TicketAttachs { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }


        public virtual ApplicationUser OwnerUser { get; set; }
        
        public virtual ApplicationUser AssignedToUser { get; set; }

        

        public virtual Project Project { get; set; }
        
        public virtual TicketStatus  TicketStatus { get; set; }

        public virtual TicketPriority TicketPriority { get; set; }

        public virtual TicketType TicketType { get; set; }

    }
}