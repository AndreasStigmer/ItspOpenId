using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace OAuthServer.App_Start
{
    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection config)
        {
            config.IgnoreRoute("{resource}.axd/{*pathInfo}");

            config.MapRoute(
                name:"Default",
                url:"identity/{controller}/{action}",
                defaults:new {controller="Account",action="index" }
                );
        }
    }
}
