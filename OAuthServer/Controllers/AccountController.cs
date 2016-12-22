using OAuthServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserRepo;

namespace OAuthServer.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Create(string signin)
        {
            return View(new NewAccountModel());
        }

        [HttpPost]
        public ActionResult Create(string signin, NewAccountModel model)
        {
            using (var repo=new UserRepo.UserRepository())
            {

                User nuser = new UserRepo.User();
                nuser.Subject = Guid.NewGuid().ToString();
                nuser.UserName = model.Username;
                nuser.Password = model.Password;
                nuser.IsActive = true;

                nuser.UserClaims.Add(new UserClaim()
                {
                    ClaimType = "given_name",
                    ClaimValue = model.FirstName,
                    Subject = nuser.Subject,
                    Id = Guid.NewGuid().ToString()
                });
                nuser.UserClaims.Add(new UserClaim()
                {
                    ClaimType = "family_name",
                    ClaimValue = model.LastName,
                    Subject = nuser.Subject,
                    Id = Guid.NewGuid().ToString()
                });

                repo.AddUser(nuser);


        }
            return Redirect("~/identity/login?signin="+signin);
        }
    }
}