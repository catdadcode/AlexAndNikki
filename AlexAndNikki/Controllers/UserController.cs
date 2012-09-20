using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlexAndNikki.Models;
using System.Web.Security;

namespace AlexAndNikki.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        AlexAndNikkiDBEntities db = new AlexAndNikkiDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogOn()
        {
            return View();
        }

        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Blog");
        }

        [HttpPost]
        public RedirectToRouteResult LogOn(string Username, string Password)
        {
            AlexAndNikki.Models.User user = db.Users.Where(x => x.Username.ToLower() == Username.ToLower()).FirstOrDefault();
            if (user != null)
            {
                if (Password == user.Password)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("LogOn");
                }
            }
            else
            {
                return RedirectToAction("LogOn");
            }
        }

    }
}
