using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using UserRepo;
using System.IdentityModel.Claims;

namespace OAuthServer.Services
{
    class CustomUserService:UserServiceBase
    {
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            User loginuser;
            using (var repo = new UserRepository())
            {
                //Hämtar en User baserat på Username och Password
                loginuser = repo.GetUser(context.UserName, context.Password);

                if (loginuser == null)
                {
                    //Skapar ett Error AuthenticateResult
                    context.AuthenticateResult = new AuthenticateResult("Invalid credentials");
                    return Task.FromResult(0);
                }

                //Skapar ett authenticate result av en user efte rlyckad inloggning
                context.AuthenticateResult = new AuthenticateResult(loginuser.Subject, loginuser.UserClaims.FirstOrDefault(c => c.ClaimType == "given_name").ClaimValue);
               
                return Task.FromResult(0);
            }
        }

        /// <summary>
        /// Metoden används för att returnera de Claims som klienten efterfrågar. Klienten
        /// efterfrågar Scopes och Scopets claims returneras för den aktuelle användaren.
        /// Context objektets subject property är den aktuelle användaren som en ClaimsPrincipal
        /// 
        /// Metoden anropas normalt sett flera gånger under en authensieringsprocess.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using(var repo=new UserRepository())
            {

                //Hämtar en användare ur repositoryn baserat på subject
                var user = repo.GetUser(context.Subject.FindFirst("sub").Value);

                //Skapar en List av Claims och lägger till ett subject claim
                var claims = new List<System.Security.Claims.Claim>() {
                    new System.Security.Claims.Claim("sub",user.Subject)
                };

                //Lägger till samtliga claims en user har genom att projicera userclaims till claims
                claims.AddRange(user.UserClaims.Select(uc => new System.Security.Claims.Claim(uc.ClaimType,uc.ClaimValue)));

                
                //Kontrollera om alla claims begärts eller inte
                if(!context.AllClaimsRequested) { 
                    
                    //Ett claim returneras endast om dess ClaimType ingår i listan av rewquested Claims
                    claims = claims.Where(d => context.RequestedClaimTypes.Contains(d.Type)).ToList();
                }

                context.IssuedClaims = claims;
                return Task.FromResult(0);
            }
            
        }


        /// <summary>
        /// Denn ametod används för att kontrollera om en användare är registrerade som inaktiv
        /// eller aktiv. Kan användas om en användare skall konfirmera epostadress eller inte
        /// betalat sin avgift mm
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task IsActiveAsync(IsActiveContext context)
        {
            
            using(var repo=new UserRepository())
            {
                var user = repo.GetUser(context.Subject.FindFirst("sub").Value);
                context.IsActive = user.IsActive;
            }

            return Task.FromResult(0);

        }
    }
}
