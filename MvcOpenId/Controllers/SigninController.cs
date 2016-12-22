using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOpenId.Controllers
{
    public class SigninController : Controller
    {
        // GET: Signin
        public ActionResult Logout()
        {
            //Loggar ut ur owinOcntexten
            Request.GetOwinContext().Authentication.SignOut();
            return View();
        }
    }
}