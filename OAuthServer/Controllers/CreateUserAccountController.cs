using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAuthServer.Controllers
{
    public class CreateUserAccountController : Controller
    {
        // GET: CreateUserAccount
        public ActionResult Index(string signin)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Models.NewUserModel newUser)
        {
            var repo=new UserRepository.UserRepository();
            UserRepository.User u = new UserRepository.User();
            u.Subject = new Guid().ToString();
            u.UserName = newUser.UserName;
            u.Password = newUser.Password;
            u.UserClaims.Add(new UserRepository.UserClaim { ClaimType = "given_name", ClaimValue = newUser.FirstName });
            u.UserClaims.Add(new UserRepository.UserClaim { ClaimType = "family_name", ClaimValue = newUser.FirstName });
            u.UserClaims.Add(new UserRepository.UserClaim { ClaimType = "email", ClaimValue = newUser.FirstName });
            repo.
        }
    }
}