using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using ProjConstants;
using System.Security.Claims;
using IdentityModel.Client;
using System.Diagnostics;
using Serilog;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Web.Helpers;
using IdentityModel;

[assembly: OwinStartup(typeof(MvcOpenId.Startup))]

namespace MvcOpenId
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = IdentityModel.JwtClaimTypes.Name;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Trace().CreateLogger();

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions {
                AuthenticationType = "cookie",
                ExpireTimeSpan = new TimeSpan(0, 30, 0),
                SlidingExpiration=true,
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions() {
                Authority = Uris.STS,
                ClientId="MvcOpenId",
                ClientSecret="hemligt",
                ResponseType="code id_token token",
                PostLogoutRedirectUri=Uris.MvcOpenIdCallback,
                SignInAsAuthenticationType="cookie",
                Scope="openid profile email address roles read offline_access",
                RedirectUri=Uris.MvcOpenIdCallback,
                Notifications=new OpenIdConnectAuthenticationNotifications() {
                    SecurityTokenValidated=async n => {
                        ClaimsIdentity ci = new ClaimsIdentity(n.AuthenticationTicket.Identity.AuthenticationType, "name", "role");

                        //ClaimsIdentity ci = n.AuthenticationTicket.Identity as ClaimsIdentity;
                        UserInfoClient ui = new UserInfoClient(Uris.STSUserInfoEndpoint);
                        UserInfoResponse ur= await ui.GetAsync(n.ProtocolMessage.AccessToken);
                        foreach (Claim c in ur.Claims) { 
                            Debug.WriteLine(c.Type + "--" + c.Value);
                        }


                        //Fixar fram en refreshtoken som kan användas när access_token går ut
                        TokenClient tc = new TokenClient(Uris.STSTokenEndpoint, "MvcOpenId", "hemligt");
                        var refreshresponse = await tc.RequestAuthorizationCodeAsync(n.ProtocolMessage.Code, Uris.MvcOpenIdCallback);

                        //Räknar ut tiden för expire ac access_token baserat på utc tid
                        var expire = DateTime.UtcNow.AddSeconds(refreshresponse.ExpiresIn).ToString();
                        ci.AddClaim(new Claim("access_token", refreshresponse.AccessToken));
                        ci.AddClaim(new Claim("expires", expire));
                        //Sparar refreshtoken som ett claim
                        ci.AddClaim(new Claim("refresh_token", refreshresponse.RefreshToken));

                        ci.AddClaim(new Claim("id_token",refreshresponse.IdentityToken));
                        ///////////////////////////////////////////////////////////////////////////

                        ci.AddClaims(ur.Claims);
                        var subj = n.AuthenticationTicket.Identity.FindFirst(JwtClaimTypes.Subject).Value;
                        var iss = n.AuthenticationTicket.Identity.FindFirst(JwtClaimTypes.Issuer).Value;
                        var NameClaim = new Claim(JwtClaimTypes.Name, iss + subj);
                        ci.AddClaim(NameClaim);

                        n.AuthenticationTicket = new Microsoft.Owin.Security.AuthenticationTicket(ci, n.AuthenticationTicket.Properties);
                    },
                    RedirectToIdentityProvider= async n=> {

                        if(n.ProtocolMessage.RequestType==Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idhint = n.OwinContext.Authentication.User.FindFirst("id_token");
                            if(idhint!=null)
                            {
                                n.ProtocolMessage.IdTokenHint = idhint.Value;

                            }
                        }

                    }

                }
            });
        }
    }
}
