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
        public ActionResult Index()
        {
            string def= "#mvpbuzz OR #MVP13 OR #MVPBUZZ";
            string query = Session["term"] as string ?? def;
            ViewBag.Term = (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query)) ? def : query;

            List<string> terms = Session["recent"] as List<string> ?? new List<string>();
            if (!terms.Contains(query) && query!=def)
            {
                terms.Add(query);
            }

            Session["recent"] = terms;

            return View();
        }

        public ActionResult View(string s)
        {
            Session["term"] = s;
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
           var url= String.Format("http://otter.topsy.com/search.json?q={0}&window={1}&type={2}&apikey=2D33HFEUDVGQGLZAOQJQAAAAACGLANA4ENIQAAAAAAAFQGYA&perpage=100",HttpUtility.UrlEncode(term),window,type);
           dynamic obj = HttpContext.Cache[url];

           if (obj == null)
           {
               using (var wc = new WebClient())
               {
                   string json = wc.DownloadString(url);
                   obj = JsonConvert.DeserializeObject(json);
                   HttpContext.Cache.Insert(url, obj, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
               }
           }
            return PartialView(obj);
        }





    }
}
