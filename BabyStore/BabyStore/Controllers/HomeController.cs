using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BabyStore.Controllers {
   public class HomeController : Controller {
      // GET: Home
      public ActionResult Index() {
         ViewBag.Hello = "Hello fro MVC";
         return View();
      }
   }
}