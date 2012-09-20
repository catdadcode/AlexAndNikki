using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexAndNikki.Models
{
    public class BlogIndexViewModel
    {
        public IList<BlogPost> BlogPosts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}