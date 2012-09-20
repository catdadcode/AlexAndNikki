using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlexAndNikki.Models;
using System.Text;

namespace AlexAndNikki.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        Random theRandom = new Random();
        AlexAndNikkiDBEntities db = new AlexAndNikkiDBEntities();

        public ViewResult Index()
        {
            return View();
        }

        #region -= Blog Management =-

        public ViewResult ManageBlogPosts()
        {
            var blogs = db.BlogPosts.Where(x => x.Author.ToLower() == User.Identity.Name.ToLower())
                .OrderByDescending(x => x.Date);
            return View(blogs);
        }

        [HttpGet]
        public ViewResult AddBlogPost()
        {
            ViewData["type"] = "add";
            return View("ManageBlogPost");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ViewResult AddBlogPost(BlogPost blogPost)
        {
            ViewData["type"] = "add";
            blogPost.FriendlyUrl = Url.ToFriendlyUrl(blogPost.Title);
            blogPost.Author = User.Identity.Name;
            blogPost.Date = DateTime.Now;
            if (!ModelState.IsValid)
                return View("ManageBlogPost");

            db.BlogPosts.AddObject(blogPost);
            db.SaveChanges();
            ViewData["message"] = "Blog post added successfully.";
            ViewData["returnUrl"] = Url.Action("ManageBlogPosts");
            return View("Result");
        }

        [HttpGet]
        public ViewResult EditBlogPost(int Id)
        {
            ViewData["type"] = "edit";
            BlogPost blogPost = db.BlogPosts.Where(x => x.Id == Id).FirstOrDefault();
            if (blogPost == null)
            {
                ViewData["message"] = "Blog post not found.";
                ViewData["returnUrl"] = Url.Action("ManageBlogPosts");
                return View("Result");
            }
            return View("ManageBlogPost", blogPost);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ViewResult EditBlogPost(int Id, FormCollection collection)
        {
            ViewData["type"] = "edit";
            BlogPost blogPost = db.BlogPosts.Single(x => x.Id == Id);
            TryUpdateModel(blogPost, collection);
            if (!ModelState.IsValid)
                return View("ManageBlogPost");
            db.SaveChanges();
            ViewData["message"] = "Blog post edited successfully.";
            ViewData["returnUrl"] = Url.Action("ManageBlogPosts");
            return View("Result");
        }

        public ViewResult DeleteBlogPost(int Id)
        {
            BlogPost blogPost = db.BlogPosts.Where(x => x.Id == Id).FirstOrDefault();
            if (blogPost == null)
            {
                ViewData["message"] = "Blog post not found.";
                ViewData["returnUrl"] = Url.Action("ManageBlogPosts");
                return View("Result");
            }
            db.BlogPosts.DeleteObject(blogPost);
            db.SaveChanges();
            ViewData["message"] = "Blog post deleted successfully.";
            ViewData["returnUrl"] = Url.Action("ManageBlogPosts");
            return View("Result");
        }

        #endregion

        #region -= Invite Management =-

        public ViewResult ManageInvites()
        {
            return View(db.Invites.OrderBy(x => x.RelatedFamily).AsEnumerable());
        }

        [HttpGet]
        public ViewResult AddInvite()
        {
            ViewBag.SelectList = RelatedFamilyList();
            
            ViewData["type"] = "add";
            return View("ManageInvite");
        }

        [HttpPost]
        public ViewResult AddInvite(Invite invite)
        {
            ViewData["type"] = "add";
            invite.Id= GenerateRandomCode();
            while (db.Invites.Any(x => x.Id == invite.Id))
                invite.Id = GenerateRandomCode();
            if (!ModelState.IsValid)
            {
                ViewBag.SelectList = RelatedFamilyList();
                return View("ManageInvite");
            }
            db.Invites.AddObject(invite);
            db.SaveChanges();
            ViewData["message"] = "Invite added successfully.";
            ViewData["returnUrl"] = Url.Action("ManageGuests", new { Id = invite.Id });
            return View("Result");
        }

        [HttpGet]
        public ViewResult EditInvite(string Id)
        {
            ViewData["type"] = "edit";
            Invite invite = db.Invites.Where(x => x.Id == Id).FirstOrDefault();
            if (invite == null)
            {
                ViewData["message"] = "Invite not found.";
                ViewData["returnUrl"] = Url.Action("ManageInvites");
                return View("Result");
            }
            ViewBag.SelectList = RelatedFamilyList();
            return View("ManageInvite", invite);
        }

        [HttpPost]
        public ViewResult EditInvite(string Id, FormCollection collection)
        {
            ViewData["type"] = "edit";
            Invite invite = db.Invites.Single(x => x.Id == Id);
            TryUpdateModel(invite, collection);
            if (!ModelState.IsValid)
            {
                ViewBag.SelectList = RelatedFamilyList();
                return View("ManageInvite");
            }
            db.SaveChanges();
            ViewData["message"] = "Invite edited successfully.";
            ViewData["returnUrl"] = Url.Action("ManageInvites");
            return View("Result");
        }

        public ViewResult DeleteInvite(string Id)
        {
            Invite invite = db.Invites.Where(x => x.Id == Id).FirstOrDefault();
            if (invite == null)
            {
                ViewData["message"] = "Invite not found.";
                ViewData["returnUrl"] = Url.Action("ManageInvites");
                return View("Result");
            }
            db.Invites.DeleteObject(invite);
            db.SaveChanges();
            ViewData["message"] = "Invite deleted successfully.";
            ViewData["returnUrl"] = Url.Action("ManageInvites");
            return View("Result");
        }

        private SelectList RelatedFamilyList()
        {
            Dictionary<string, string> listItems = new Dictionary<string, string>()
            {
                { "none", "Please Choose" },
                { "Ford", "Ford" },
                { "Sommer", "Sommer"}
            };
            return new SelectList(listItems, "key", "value");
        }

        private string GenerateRandomCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, 5)
                          .Select(s => s[theRandom.Next(s.Length)])
                          .ToArray());
            return result;
        }

        #endregion

        #region -= Guest Management =-

        public ViewResult ManageGuests(string Id)
        {
            var invite = db.Invites.Single(x => x.Id == Id);
            return View(invite);
        }

        [HttpGet]
        public ViewResult AddGuest(string Id)
        {
            Invite invite = db.Invites.Single(x => x.Id == Id);
            ViewData["type"] = "add";
            return View("ManageGuest", new Guest { InviteID = Id, Invite = invite });
        }

        [HttpPost]
        public ViewResult AddGuest(Guest guest, string Id)
        {
            guest.InviteID = Id;
            Invite invite = db.Invites.Single(x => x.Id == Id);
            if (!ModelState.IsValid)
                return View("ManageGuest", new Guest { InviteID = Id, Invite = invite });
            db.Guests.AddObject(guest);
            db.SaveChanges();
            ViewData["message"] = "Guest added successfully.";
            ViewData["returnUrl"] = Url.Action("ManageGuests", new { Id = guest.InviteID });
            return View("Result");
        }

        [HttpGet]
        public ViewResult EditGuest(short Id)
        {
            ViewData["type"] = "edit";
            Guest guest = db.Guests.Where(x => x.GuestID == Id).FirstOrDefault();
            if (guest == null)
            {
                ViewData["message"] = "Guest not found.";
                ViewData["returnUrl"] = Url.Action("ManageInvites");
                return View("Result");
            }
            return View("ManageGuest", guest);
        }

        [HttpPost]
        public ViewResult EditGuest(short Id, FormCollection collection)
        {
            ViewData["type"] = "edit";
            Guest guest = db.Guests.Single(x => x.GuestID == Id);
            TryUpdateModel(guest, collection);
            if (!ModelState.IsValid)
                return View("ManageGuest");
            db.SaveChanges();
            ViewData["message"] = "Guest edited successfully.";
            ViewData["returnUrl"] = Url.Action("ManageGuests", new { Id = guest.InviteID });
            return View("Result");
        }

        public ViewResult DeleteGuest(short Id)
        {
            Guest guest = db.Guests.Where(x => x.GuestID == Id).FirstOrDefault();
            if (guest == null)
            {
                ViewData["message"] = "Guest not found.";
                ViewData["returnUrl"] = Url.Action("ManageInvites");
                return View("Result");
            }
            string inviteId = guest.InviteID;
            db.Guests.DeleteObject(guest);
            db.SaveChanges();
            ViewData["message"] = "Guest deleted successfully.";
            ViewData["returnUrl"] = Url.Action("ManageGuests", new { Id = inviteId });
            return View("Result");
        }

        #endregion

        #region -= RSVP Status =-

        public ViewResult RSVPStatus()
        {
            RSVPStatusViewModel rsvpStatusViewModel = new RSVPStatusViewModel
            {
                TotalGuestCount = db.Guests.Count(),
                InvitedToCeremonyCount = db.Guests.Where(x => x.InvitedToCeremony).Count(),
                CeremonyYESCount = db.Guests.Where(x => x.ConfirmCeremony.ToUpper() == "YES").Count(),
                CeremonyNOCount = db.Guests.Where(x => x.ConfirmCeremony.ToUpper() == "NO").Count(),
                CeremonyMAYBECount = db.Guests.Where(x => x.ConfirmCeremony.ToUpper() == "MAYBE").Count(),
                CeremonyNoAnswerCount = db.Guests.Where(x => x.ConfirmCeremony.ToUpper() == "NO ANSWER").Count(),
                ReceptionYESCount = db.Guests.Where(x => x.ConfirmReception.ToUpper() == "YES").Count(),
                ReceptionNOCount = db.Guests.Where(x => x.ConfirmReception.ToUpper() == "NO").Count(),
                ReceptionMAYBECount = db.Guests.Where(x => x.ConfirmReception.ToUpper() == "MAYBE").Count(),
                ReceptionNoAnswerCount = db.Guests.Where(x => x.ConfirmReception.ToUpper() == "NO ANSWER").Count(),
                Invites = db.Invites.ToList().OrderBy(x => x.RelatedFamily).ThenBy(x => x.PrimaryContactNames())
            };
            return View(rsvpStatusViewModel);
        }

        #endregion
    }
}
