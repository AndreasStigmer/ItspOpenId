using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcClientCredentials.Controllers
{
    public class SecretController : Controller
    {
        // GET: Secret
        public async Task<ActionResult> Index()
        {
            HttpClient h = Client.HttpHelper.GetClient();

            string data = await h.GetStringAsync("/api/profiles/get");
            ViewBag.data = data;
            return View();
        }
    }
}