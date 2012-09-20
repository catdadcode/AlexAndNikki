using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexAndNikki.Models
{
    public class RSVPStatusViewModel
    {

        public int TotalGuestCount { get; set; }
        public int InvitedToCeremonyCount { get; set; }
        public int CeremonyYESCount { get; set; }
        public int CeremonyNOCount { get; set; }
        public int CeremonyMAYBECount { get; set; }
        public int CeremonyNoAnswerCount { get; set; }
        public int ReceptionYESCount { get; set; }
        public int ReceptionNOCount { get; set; }
        public int ReceptionMAYBECount { get; set; }
        public int ReceptionNoAnswerCount { get; set; }
        public IEnumerable<Invite> Invites { get; set; }
    }
}