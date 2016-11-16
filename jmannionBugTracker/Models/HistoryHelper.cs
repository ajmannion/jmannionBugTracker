using jmannionBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jmannionBugTracker.Models
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        TicketHistory ticketHistory =  new TicketHistory();
        Ticket ticket = new Ticket();

        public void AddHistory (int TicketId, string UpdateProperty, string oldValue, string newValue, string userId)
        {
            ticketHistory.TicketId = TicketId;
            ticketHistory.Property = UpdateProperty;
            ticketHistory.OldValue = oldValue;
            ticketHistory.NewValue = newValue;
            ticketHistory.UserId = userId;
            ticketHistory.Changed = DateTime.Now;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
            return;

        }   
      }
}