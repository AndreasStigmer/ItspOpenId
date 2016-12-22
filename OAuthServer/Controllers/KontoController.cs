using OAuthServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserRepo;

namespace OAuthServer.Controllers
{
    public class KontoController : Controller
    {
        // GET: Konto
        [HttpGet]
        public ActionResult Nytt()
        {
            return View(new NewUserModel());
        }

        [HttpPost]
        public ActionResult Nytt(string signin,NewUserModel newUser)
        {
            User u = new UserRepo.User();
            u.UserName = newUser.UserName;
            u.Password = newUser.Password;
            u.IsActive = true;
            u.Subject = Guid.NewGuid().ToString();
            u.UserClaims.Add(new UserClaim() {
                Id = Guid.NewGuid().ToString(),
                Subject=u.Subject,
                ClaimType="given_name",
                ClaimValue=newUser.FirstName
            });

            u.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = u.Subject,
                ClaimType = "family_name",
                ClaimValue = newUser.LastName
            });

            UserRepositoryDb repo = new UserRepositoryDb();
            repo.AddUser(u);

            return Redirect("/identity/login?signin="+signin);
        }
    }
}