using Model;
using MvcOpenId.Helpers;
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
        /// <summary>
        /// Hämtar OwinCoanvändare
        /// </summary>
        /// <returns></returns>
       /* public async Task<ActionResult> Index()
        {
            Member m=await Helpers.CurrentMember.Get();
            return View(m);
        }*/

        /// <summary>
        /// Hämtar en member baserad på id't som kommer som querystring
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string id)
        {
            if (id == null)
            {
                id = User.Identity.Name;
            }
            else {
                id = HttpUtility.UrlDecode(id);
            }
            ApiHelper helper = new ApiHelper();
            Member m= await helper.GetMember(id);
            return View(m);
        }

        /// <summary>
        /// Visar formulär för att skapa ett nytt statusmeddelande
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateMessage() {

            return View();
        }


        /// <summary>
        /// Sparar ner ett nytt meddelande för usern genom apiet
        /// </summary>
        /// <param name="newMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateMessage(StatusMessage newMessage)
        {
            Member m = await Helpers.CurrentMember.Get();
            newMessage.OwnerId = m.UserId;
            m.Messages.Add(newMessage);
            Helpers.ApiHelper helper = new Helpers.ApiHelper();
            await helper.SaveMember(m);
            return RedirectToAction("index");
        }
    }
}