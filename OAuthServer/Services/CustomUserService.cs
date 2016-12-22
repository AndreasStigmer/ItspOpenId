using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using System.Security.Claims;
using UserRepository;

namespace OAuthServer.Services
{
    public class CustomUserService : UserServiceBase
    {
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            using (var userRepo = new UserRepository.UserRepository())
            {
                var user = userRepo.GetUser(context.UserName, context.Password);
                if(user==null)
                {
                    context.AuthenticateResult = new AuthenticateResult("Felaktiga inloggningsuppgifter!");
                    return Task.FromResult(0);
                }
                context.AuthenticateResult = new AuthenticateResult(user.Subject, user.UserClaims.First(c => c.ClaimType == Constants.ClaimTypes.GivenName).ClaimValue);

                return Task.FromResult(0);
            }
        }
        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {

            using (var userRepository=new UserRepository.UserRepository())
            {
                //Hämta usern baserat på subject
                var user = userRepository.GetUser(context.Subject.GetSubjectId());

                //Skapa en claimslista och sätt sub claim
                var claims = new List<Claim>
                {
                    new Claim(Constants.ClaimTypes.Subject,user.Subject)
                };

                //Hämtar resten av userns claims
                claims.AddRange(user.UserClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)));
                
                //Om inte alla claims har begärts
                //hämta endast de claims som inte finns med i context.RequestedClaimTypes
                if(!context.AllClaimsRequested)
                {
                    claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                }

                //Sätt issued claims på contextobjektet
                context.IssuedClaims = claims;
                return Task.FromResult(0);
            }
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            using (var userRep=new UserRepository.UserRepository())
            {
                if(context.Subject==null)
                {
                    throw new ArgumentNullException();
                }
                //Hämtar user
                var user = userRep.GetUser(context.Subject.GetSubjectId());
                //Sätter hurvida usern är aktiv eller inte
                context.IsActive = (user != null) && user.IsActive;

                return Task.FromResult(0);
            }
            
        }
    }
}
