using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using Serilog;

[assembly: OwinStartup(typeof(ApiAuth.App_Start.Startup))]

namespace ApiAuth.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Trace().CreateLogger();

            IdentityServerBearerTokenAuthenticationOptions options = new IdentityServerBearerTokenAuthenticationOptions();
            options.Authority = "https://localhost:44372/identity";
            options.RequiredScopes = new[] { "read" };
            options.IssuerName = "https://MyISSUER:44372/";

            
            
            app.UseIdentityServerBearerTokenAuthentication(options);

            HttpConfiguration conf = new HttpConfiguration();
            WebApiConfig.Register(conf);
            app.UseWebApi(conf);
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
