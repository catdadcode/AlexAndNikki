using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlexAndNikki.Models;
using System.Text;

namespace AlexAndNikki.Controllers
{
    public class PhotosAndVideoController : Controller
    {
        AlexAndNikkiDBEntities db = new AlexAndNikkiDBEntities();

        public PhotosAndVideoController()
        {
            ViewData["SelectedLink"] = "PhotosAndVideo";
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
