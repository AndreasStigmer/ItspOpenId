using OAuthServer.Models;
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
        public ActionResult Index()
        {
            return View(new NewUserModel());
        }
        [HttpPost]
        public ActionResult Index(string signin,NewUserModel newUser)
        {
            var repo=new UserRepository.UserRepository();
            UserRepository.User u = new UserRepository.User();
            u.Subject = Guid.NewGuid().ToString();
            u.UserName = newUser.UserName;
            u.IsActive = true;
            u.Password = newUser.Password;
            u.UserClaims.Add(
                new UserRepository.UserClaim {
                    Id =Guid.NewGuid().ToString(),
                    Subject =u.Subject,
                    ClaimType = "given_name",
                    ClaimValue = newUser.FirstName
                });

            u.UserClaims.Add(
                new UserRepository.UserClaim {
                    Id = Guid.NewGuid().ToString(),
                    Subject = u.Subject,
                    ClaimType = "family_name",
                    ClaimValue = newUser.FirstName
                });

            u.UserClaims.Add(
                new UserRepository.UserClaim {
                    Id = Guid.NewGuid().ToString(),
                    Subject = u.Subject,
                    ClaimType = "email",
                    ClaimValue = newUser.FirstName
                });
            UserRepository.UserRepository ur = new UserRepository.UserRepository();
            ur.AddUser(u);

            return Redirect("~/identity/login?signin=" + signin);


        }
    }
}