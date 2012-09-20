using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlexAndNikki.Models;

namespace AlexAndNikki.Controllers
{
    public class BlogController : Controller
    {
        public BlogController()
        {
            ViewData["SelectedLink"] = "Blog";
        }

        public ViewResult Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            AlexAndNikkiDBEntities dbContext = new AlexAndNikkiDBEntities();

            var blogPostsToShow = dbContext.BlogPosts;

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = 5,
                TotalItems = blogPostsToShow.Count()
            };

            var viewModel = new BlogIndexViewModel
            {
                BlogPosts = blogPostsToShow
                .OrderByDescending(x => x.Date)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToList(),
                PagingInfo = pagingInfo
            };

            return View("Blog", viewModel);
        }

        public ViewResult Post(string Id)
        {
            AlexAndNikkiDBEntities db = new AlexAndNikkiDBEntities();
            BlogPost post = db.BlogPosts.Where(x => x.FriendlyUrl == Id).FirstOrDefault();

            return View(post);
        }

        [HttpPost]
        public RedirectToRouteResult AddComment(int BlogId, string Name, string Website, string Comment)
        {
            AlexAndNikkiDBEntities db = new AlexAndNikkiDBEntities();
            BlogPost post = db.BlogPosts.Where(x => x.Id == BlogId).First();
            post.BlogPostComments.Add(new BlogPostComment
            {
                BlogPostId = BlogId,
                Name = Name,
                Website = Website,
                Comment = Comment,
                Date = DateTime.Now
            });
            db.SaveChanges();
            return RedirectToAction("Post", new { Id = post.FriendlyUrl });
        }
    }
}
