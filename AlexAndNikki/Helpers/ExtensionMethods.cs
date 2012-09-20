using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;

namespace AlexAndNikki.Helpers
{
    public static class ExtensionMethods
    {
        public static string FormatBody(this string text)
        {
            text = text.Replace("\r", "<br />").Replace("\n", "");
            return text;
        }

        public static MvcHtmlString StateDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            Dictionary<string, string> stateList = new Dictionary<string, string>()
            {
                {"None","Please Choose"},
                {"AL", "Alabama"},
                {"AK", "Alaska"},
                {"AZ", "Arizona"},
                {"AR", "Arkansas"},
                {"CA", "California"},
                {"CO", "Colorado"},
                {"CT", "Connecticut"},
                {"DE", "Delaware"},
                {"FL", "Florida"},
                {"GA", "Georgia"},
                {"HI", "Hawaii"},
                {"ID", "Idaho"},
                {"IL", "Illinois"},
                {"IN", "Indiana"},
                {"IA", "Iowa"},
                {"KS", "Kansas"},
                {"KY", "Kentucky"},
                {"LA", "Louisiana"},
                {"ME", "Maine"},
                {"MD", "Maryland"},
                {"MA", "Massachusetts"},
                {"MI", "Michigan"},
                {"MN", "Minnesota"},
                {"MS", "Mississippi"},
                {"MO", "Missouri"},
                {"MT", "Montana"},
                {"NE", "Nebraska"},
                {"NV", "Nevada"},
                {"NH", "New Hampshire"},
                {"NJ", "New Jersey"},
                {"NM", "New Mexico"},
                {"NY", "New York"},
                {"NC", "North Carolina"},
                {"ND", "North Dakota"},
                {"OH", "Ohio"},
                {"OK", "Oklahoma"},
                {"OR", "Oregon"},
                {"PA", "Pennsylvania"},
                {"RI", "Rhode Island"},
                {"SC", "South Carolina"},
                {"SD", "South Dakota"},
                {"TN", "Tennessee"},
                {"TX", "Texas"},
                {"UT", "Utah"},
                {"VT", "Vermont"},
                {"VA", "Virginia"},
                {"WA", "Washington"},
                {"WV", "West Virginia"},
                {"WI", "Wisconsin"},
                {"WY", "Wyoming"}
            };
            return html.DropDownListFor(expression, new SelectList(stateList, "key", "value"));
        }
    }
}