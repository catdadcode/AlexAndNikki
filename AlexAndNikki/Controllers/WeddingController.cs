using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlexAndNikki.Models;
using System.Text;

namespace AlexAndNikki.Controllers
{
    public class WeddingController : Controller
    {
        AlexAndNikkiDBEntities db = new AlexAndNikkiDBEntities();

        public WeddingController()
        {
        }

        public ActionResult Index()
        {
            return RedirectToAction("Information");
        }

        public ActionResult Information()
        {
            ViewData["SelectedLink"] = "WeddingInformation";
            return View();
        }

        public ViewResult RSVP(string Id)
        {
            ViewData["SelectedLink"] = "WeddingRSVP";
            if (String.IsNullOrEmpty(Id))
                return View("EnterCode");
            else
            {
                Invite invite = db.Invites.SingleOrDefault(x => x.Id == Id);
                if (invite != null)
                    return View(invite);
                else
                {
                    ViewData["message"] = "Invite not found.";
                    ViewData["returnUrl"] = Url.Action("RSVP");
                    return View("Result");
                }
            }
        }

        public ViewResult SaveRSVP(Invite invite)
        {
            ViewData["SelectedLink"] = "WeddingRSVP";
            foreach (Guest guestData in invite.Guests)
            {
                Guest guest = db.Guests.Single(x => x.GuestID == guestData.GuestID);
                if (guest.InvitedToCeremony)
                    guest.ConfirmCeremony = guestData.ConfirmCeremony;
                guest.ConfirmReception = guestData.ConfirmReception;
            }
            db.SaveChanges();
            ViewBag.InviteId = invite.Id;
            return View("RSVPThankYou");
        }

        [HttpGet]
        public ViewResult AddGuest(string Id)
        {
            ViewData["SelectedLink"] = "WeddingRSVP";
            Invite invite = db.Invites.Single(x => x.Id == Id);
            return View(new Guest { Invite = invite, InviteID = Id });
        }

        [HttpPost]
        public ViewResult AddGuest(Guest guest)
        {
            ViewData["SelectedLink"] = "WeddingRSVP";
            guest.ConfirmReception = "No Answer";
            guest.ConfirmCeremony = "No Answer";
            
            if (!ModelState.IsValid)
            {
                Invite invite = db.Invites.Single(x => x.Id == guest.InviteID);
                return View(new Guest { Invite = invite, InviteID = guest.InviteID });
            }
            db.Guests.AddObject(guest);
            db.SaveChanges();
            ViewData["message"] = "A new guest has been added to your invitation!";
            ViewData["returnUrl"] = Url.Action("RSVP", new { Id = guest.InviteID });
            return View("result");
        }
    }
}
