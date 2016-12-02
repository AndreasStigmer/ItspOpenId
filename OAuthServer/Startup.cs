using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using System.Configuration;
using Serilog;

[assembly: OwinStartup(typeof(OAuthServer.Startup))]

namespace OAuthServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var inMemeoryManager = new InMemoryManager();
            var factory = new IdentityServerServiceFactory()
                .UseInMemoryUsers(inMemeoryManager.GetUsers())
                .UseInMemoryScopes(inMemeoryManager.GetScopes())
                .UseInMemoryClients(inMemeoryManager.GetClients());
            var cert = Convert.FromBase64String(ConfigurationManager.AppSettings["SigningCertificate"]);

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            string certFile = @"C:\Users\admin.CAMPUS-F0U53S1N\Documents\Visual Studio 2015\Projects\ApiAuth\OAuthServer\localhost.pfx";
            var options = new IdentityServerOptions {
                SigningCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certFile,"password"),
                IssuerUri= "https://MyISSUER:44372/",
                SiteName="My Super AuthServer",
                PublicOrigin= "https://localhost:44372/",
                Factory =factory
            };
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Trace().CreateLogger();
            app.Map("/identity", idsrv => {

              
                idsrv.UseIdentityServer(options);
            });

            
        }
    }
}
