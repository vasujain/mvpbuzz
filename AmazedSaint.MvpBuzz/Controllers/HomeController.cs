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
            return View();
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
