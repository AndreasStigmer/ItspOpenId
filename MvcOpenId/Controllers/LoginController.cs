using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOpenId.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult SignOut()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return View();
        }
    }
}