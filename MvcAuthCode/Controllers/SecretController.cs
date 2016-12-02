using MvcAuthCode.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcAuthCode.Controllers
{
    public class SecretController : Controller
    {
        // GET: Secret
        public async Task<ActionResult> Index()
        {
            HttpClient hc = HttpHelper.GetClient();
            var data = await hc.GetStringAsync("/api/profiles/get");
            ViewBag.data = data;


            return View();
        }
    }
}