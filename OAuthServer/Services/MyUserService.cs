using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IdentityServer3.Core.Models;
using UserRepo;
using System.Security.Claims;
using IdentityServer3.Core;

namespace OAuthServer.Services
{
    public class MyUserService : UserServiceBase
    {

        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {

            using (var userRepo = new UserRepositoryDb())
            {
                //Befintlig user
                var existingLinkedUser = userRepo.GetUserForExternalUser(context.ExternalIdentity.ProviderId, context.ExternalIdentity.Provider);

                if(existingLinkedUser!=null)
                {
                    context.AuthenticateResult = new AuthenticateResult(
                        existingLinkedUser.Subject,
                        existingLinkedUser.UserClaims.First(c=>c.ClaimType=="given_name").ClaimValue,
                        existingLinkedUser.UserClaims.Select(c=>new System.Security.Claims.Claim(c.ClaimType,c.ClaimValue)),
                        authenticationMethod:Constants.AuthenticationMethods.External,
                        identityProvider:context.ExternalIdentity.Provider
                        );
                    return Task.FromResult(0);
                }
            

                var emailClaim = context.ExternalIdentity.Claims.FirstOrDefault(d => d.Type == "email");
                if(emailClaim==null)
                {
                    context.AuthenticateResult = new AuthenticateResult("no email");
                    return Task.FromResult(0);
                }
                var user = userRepo.GetUserByEmail(emailClaim.Value);
                if(user==null)
                {
                    context.AuthenticateResult = new AuthenticateResult("No matching account");
                    return Task.FromResult(0);
                }

                UserLogin ul = new UserLogin()
                {
                    ProviderKey = context.ExternalIdentity.ProviderId,
                    LoginProvider = context.ExternalIdentity.Provider,
                    Subject = user.Subject
                };
                userRepo.AddLoginToUser(user, ul);

                context.AuthenticateResult = new AuthenticateResult(
                          user.Subject,
                          user.UserClaims.First(c => c.ClaimType == "given_name").ClaimValue,
                          user.UserClaims.Select(c => new System.Security.Claims.Claim(c.ClaimType, c.ClaimValue)),
                          authenticationMethod: Constants.AuthenticationMethods.External,
                          identityProvider: context.ExternalIdentity.Provider
                          );

                return Task.FromResult(0);
            }

                
        }
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            using (var repo=new UserRepositoryDb())
            {
                User u = repo.GetUser(context.UserName, context.Password);
                if(u==null)
                {
                    context.AuthenticateResult = new AuthenticateResult("Fel uppgifter");
                    return Task.FromResult(0);
                }
                context.AuthenticateResult = new AuthenticateResult(u.Subject, u.UserClaims.First(f => f.ClaimType == "given_name").ClaimValue);
                return Task.FromResult(0);
            }
        }


        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using (var repo = new UserRepositoryDb())
            {
                User u = repo.GetUser(context.Subject.FindFirst("sub").Value);
                var claims = new List<Claim>() {
                    new Claim("sub",u.Subject)
                };
                claims.AddRange(
                    u.UserClaims.Select(d => 
                    new Claim(d.ClaimType, d.ClaimValue)).ToList());
                if(!context.AllClaimsRequested)
                {
                    claims = claims.Where(
                        d => context.RequestedClaimTypes.Contains(d.Type))
                        .ToList();

                }
                context.IssuedClaims = claims;
                return Task.FromResult(0);

            }

            
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            using (var repo = new UserRepositoryDb())
            {
                User u = repo.GetUser(context.Subject.FindFirst("sub").Value);
                context.IsActive = u.IsActive;

            }
            return Task.FromResult(0);
        }

        }
}