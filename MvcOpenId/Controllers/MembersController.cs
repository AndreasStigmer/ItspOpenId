using Model;
using MvcOpenId.Helpers;
using ProjConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcOpenId.Controllers
{
    public class MembersController : Controller
    {
        // GET: Secret
        [Authorize]
        public async Task<ActionResult> Index()
        {
            ClaimsIdentity ci = User.Identity as ClaimsIdentity;
            Member current = await Helpers.CurrentMember.Get();
            if(current==null)
            {
               var response=await Helpers.CurrentMember.Register(ci);
            }
            HttpClient hc = ApiHelper.GetClient();
            string data = await hc.GetStringAsync("/api/member");
            List<Member> members = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Member>>(data);
            return View(members);
        }
    }
}