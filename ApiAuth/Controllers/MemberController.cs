using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataRepository;
using Model;

namespace ApiAuth.Controllers
{
    [Authorize]
    public class MemberController : ApiController
    {
        MemberRepository dr = new MemberRepository();
        public IHttpActionResult Get()  {
            
            return Json(dr.GetUsers());
        }

        public IHttpActionResult Get(string id)
        {
            return Json(dr.GetUsers().FirstOrDefault(u=>u.UserId==id));
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]Member newMember)
        {
            dr.SaveUser(newMember);
            return Json("Saved");
        }
    }
}
