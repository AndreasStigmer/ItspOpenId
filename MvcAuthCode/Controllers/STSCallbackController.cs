using IdentityModel.Client;
using ProjConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAuthCode.Controllers
{
    public class STSCallbackController : Controller
    {
        // GET: STSCallback
        public ActionResult Index()
        {
            var code = Request.QueryString["code"];
            TokenClient tc = new TokenClient(Uris.STSTokenEndpoint, "MvcAuthCode", "hemligt");
            var tokenresult = tc.RequestAuthorizationCodeAsync(code, Uris.MvcAuthCodeCallback).Result;
            var token = tokenresult.AccessToken;

            if (token != null) {
                Response.Cookies["mycookie"]["access_token"] = token;
            }

            Response.Redirect(Request.QueryString["state"]);

            return View();
        }
    }
}