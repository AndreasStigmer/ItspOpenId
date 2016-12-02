using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcOpenId.Controllers
{
    [Authorize]
    public class MemberhomeController : Controller
    {
        // GET: Memberhome
        public async Task<ActionResult> Index()
        {
            Member m=await Helpers.CurrentMember.Get();
            return View(m);
        }

        [HttpGet]
        public ActionResult CreateMessage() {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage(StatusMessage newMessage)
        {
            Member m = await Helpers.CurrentMember.Get();
            newMessage.OwnerId = m.UserId;
            m.Messages.Add(newMessage);
            return View();
        }
    }
}