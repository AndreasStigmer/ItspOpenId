using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using ProjConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace OAuthServer
{
    class InMemoryManager
    {

        public List<InMemoryUser> GetUsers() {
            return new List<InMemoryUser>
            {
                new InMemoryUser{
                    Subject="987074320174291898s798a790safdfd0s990",
                    Username="andreas",
                    Password="hemligt",
                    Claims=new [] {
                        new Claim(Constants.ClaimTypes.GivenName,"Andreas"),
                        new Claim(Constants.ClaimTypes.FamilyName,"Stigmer"),
                        new Claim(Constants.ClaimTypes.Address,"Drabanterörgatan 21"),
                        new Claim(Constants.ClaimTypes.Email,"andreas@campusi12.se"),
                        new Claim(Constants.ClaimTypes.Email,"andreas.stigmer@outlook.com"),
                        new Claim(Constants.ClaimTypes.Role,"admin")
                    },
                },
                new InMemoryUser{
                    Subject="73829087439287320943762987108",
                    Username="pelle",
                    Password="hemligt",
                    Claims=new [] {
                        new Claim(Constants.ClaimTypes.GivenName,"Pelle"),
                        new Claim(Constants.ClaimTypes.FamilyName,"Svensson"),
                        new Claim(Constants.ClaimTypes.Address,"Hemvägen 12"),
                        new Claim(Constants.ClaimTypes.Email,"pelle@home.se"),
                        new Claim(Constants.ClaimTypes.Role,"user")
                    },
                }
            };
        }

        public IEnumerable<Scope> GetScopes()
        {
            return new[] {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Address,
                StandardScopes.Email,
                StandardScopes.OfflineAccess,

                new Scope {
                    Name="read",
                    Type=ScopeType.Resource,
                    DisplayName ="Read user Data",
                    Claims=new List<ScopeClaim> {
                        new ScopeClaim("role",false)
                    }
                },
                new Scope {
                    Name="roles",
                    Type=ScopeType.Identity,
                    DisplayName ="Access to users site role",
                    Claims=new List<ScopeClaim> {
                        new ScopeClaim("role",false)
                    }
                }
            };
        }
        public IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId="MvcClientCredentials",
                    ClientSecrets=new List<Secret> {
                        new Secret("hemligt".Sha256())
                    },
                    ClientName="Mvc Client Credentials",
                    Flow=Flows.ClientCredentials,
                    AllowAccessToAllScopes=true,
                    Enabled =true
                },
                new Client
                {
                    ClientId="MvcAuthCode",
                    ClientSecrets=new List<Secret> {
                        new Secret("hemligt".Sha256())
                    },
                    ClientName="Mvc Client Credentials",
                    Flow=Flows.AuthorizationCode,
                    AllowAccessToAllScopes=true,
                    RedirectUris=new List<string> {
                        Uris.MvcAuthCodeCallback
                    },
                    Enabled =true,



                },
                 new Client
                {
                    ClientId="MvcOpenId",
                    ClientSecrets=new List<Secret> {
                        new Secret("hemligt".Sha256())
                    },
                    IdentityTokenLifetime=120,
                    AccessTokenLifetime=40,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    ClientName="Mvc OpenID",
                    Flow=Flows.Hybrid,
                    RequireConsent=false,
                    AllowAccessToAllScopes=true,
                    RedirectUris=new List<string> {
                        Uris.MvcOpenIdCallback
                    },
                    PostLogoutRedirectUris=new List<string>() {
                        Uris.MvcOpenIdCallback
                    },
                    Enabled =true,



                }
            };
        }
    }
}
