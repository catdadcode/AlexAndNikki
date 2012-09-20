using System;
using System.Text;
using System.Web.Mvc;
using AlexAndNikki.Models;

namespace AlexAndNikki.Models
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
        PagingInfo pagingInfo,
        Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            if (pagingInfo.TotalPages > 1)
            {
                TagBuilder first = new TagBuilder("a");
                first.MergeAttribute("href", pageUrl(1));
                first.InnerHtml = "first";
                result.AppendLine(first.ToString());
            }
            if (pagingInfo.CurrentPage > 1)
            {
                TagBuilder prev = new TagBuilder("a");
                prev.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
                prev.InnerHtml = "prev";
                result.AppendLine(prev.ToString());
            }
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("pageLinkSelected");
                result.AppendLine(tag.ToString());
            }
            if (pagingInfo.CurrentPage < pagingInfo.TotalPages)
            {
                TagBuilder next = new TagBuilder("a");
                next.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
                next.InnerHtml = "next";
                result.AppendLine(next.ToString());
            }
            if (pagingInfo.TotalPages > 1)
            {
                TagBuilder last = new TagBuilder("a");
                last.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                last.InnerHtml = "last";
                result.AppendLine(last.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}