using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace AmazedSaint.MvpBuzz.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string q = "CSharp OR MSDN OR TypeScript OR VB.NET OR #VS2012", string t = "Developers")
        {
            string query = Session["term"] as string ?? q;
            string title = Session["title"] as string ?? t;
            ViewBag.Term = (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query)) ? q : query;
            ViewBag.Title = (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title)) ? t : title;

            Dictionary<string, string> terms = Session["recent"] as Dictionary<string, string> ?? new Dictionary<string, string>();
            if (query!=q)
            {
                if (terms.Keys.Contains(query)) terms.Remove(query);
                terms.Add(query,title??query);
            }

            Session["recent"] = terms;

            return View();
        }

        public ActionResult View(string s,string title)
        {
            Session["term"] = s;
            Session["title"] = title ?? s;
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [OutputCache(Duration = 4000, VaryByParam = "term;window;type")]
        public ActionResult Render(string term,string window, string type)
        {
            var u = String.Format("http://otter.topsy.com/search.json?q={0}&window={1}&type={2}&apikey=2D33HFEUDVGQGLZAOQJQAAAAACGLANA4ENIQAAAAAAAFQGYA&perpage=100&allow_lang=en", HttpUtility.UrlEncode(term), window, type);
           dynamic obj = HttpContext.Cache[u];

           if (obj == null)
           {
               using (var w = new WebClient())
               {
                   string json = w.DownloadString(u);
                   obj = JsonConvert.DeserializeObject(json);
                   HttpContext.Cache.Insert(u, obj, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
               }
           }
            return PartialView(obj);
        }





    }
}
