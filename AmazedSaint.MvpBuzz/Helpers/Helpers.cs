using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AmazedSaint.MvpBuzz.Helpers
{
    public static class Helpers
    {
        static public IHtmlString Raw(string s)
        {
            return new HtmlString(HttpUtility.HtmlDecode(s));
        }
       
    }

    

}