using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using System.Configuration;
using Serilog;
using OAuthServer.Services;
using IdentityServer3.Core.Services;
using System.Collections.Generic;
using Microsoft.Owin.Security.Facebook;
using System.Net.Http;
using Newtonsoft.Json;

[assembly: OwinStartup(typeof(OAuthServer.Startup))]

namespace OAuthServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var inMemeoryManager = new InMemoryManager();
            var factory = new IdentityServerServiceFactory()
            //    .UseInMemoryUsers(inMemeoryManager.GetUsers())
                .UseInMemoryScopes(inMemeoryManager.GetScopes())
                .UseInMemoryClients(inMemeoryManager.GetClients());

            MyUserService users = new MyUserService();
            factory.UserService = new Registration<IUserService>(r => users);
            

            var cert = Convert.FromBase64String(ConfigurationManager.AppSettings["SigningCertificate"]);

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            string certFile = AppDomain.CurrentDomain.BaseDirectory+@"\localhost.pfx";
            var options = new IdentityServerOptions {
                SigningCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certFile,"password"),
                IssuerUri= "https://MyISSUER:44372/",
                SiteName="My Super AuthServer",
                PublicOrigin= "https://localhost:44372/",
                Factory =factory,
                AuthenticationOptions =new AuthenticationOptions()
                {
                    PostSignOutAutoRedirectDelay=5,
                    EnablePostSignOutAutoRedirect=true,
                    LoginPageLinks=new List<LoginPageLink>()
                    {
                        new LoginPageLink() {
                           Text="Skapa konto",
                           Type="createaccount",
                           Href="~/Konto/Nytt"
                        }

                    },
                    IdentityProviders=ConfigureProviders
                    
                }
                
            };


            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Trace().CreateLogger();
            app.Map("/identity", idsrv => {

              
                idsrv.UseIdentityServer(options);
            });

            
        }

        //
        private void ConfigureProviders(IAppBuilder app, string signInAsType) {
            var fbOptions = new FacebookAuthenticationOptions() {
                AuthenticationType="Facebook",
                SignInAsAuthenticationType=signInAsType,
                AppId= "1235203743213646",
                AppSecret ="f1d20fa89a7dc59635c79fd25e23a6da",
                Provider=new FacebookAuthenticationProvider() {
                    OnAuthenticated= n=>
                    {
                        using (var client = new HttpClient()) {
                            var result = client.GetAsync("https://graph.facebook.com/me?fields=first_name,last_name,email&access_token=" + n.AccessToken).Result;

                            if(result.IsSuccessStatusCode)
                            {
                                var userinfo = result.Content.ReadAsStringAsync().Result;
                                var fbuser = JsonConvert.DeserializeObject<FacebookUser>(userinfo);
                                n.Identity.AddClaim(new System.Security.Claims.Claim("given_name", fbuser.first_name));
                                n.Identity.AddClaim(new System.Security.Claims.Claim("family_name", fbuser.last_name));
                                n.Identity.AddClaim(new System.Security.Claims.Claim("email", fbuser.email));
                                
                            }
                        }


                        return Task.FromResult(0);
                    }
                }
            };
            fbOptions.Scope.Add("email");
            app.UseFacebookAuthentication(fbOptions);
        }
    }
}
