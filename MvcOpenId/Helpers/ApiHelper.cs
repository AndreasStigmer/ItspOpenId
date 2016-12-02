using ProjConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MvcOpenId.Helpers
{
    public class ApiHelper
    {

        public static HttpClient GetClient() {
            HttpClient hc = new HttpClient();
            ClaimsIdentity user = HttpContext.Current.User.Identity as ClaimsIdentity;
            var token = user.FindFirst("access_token").Value;
            hc.DefaultRequestHeaders.Clear();
            hc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            hc.SetBearerToken(token);
            hc.BaseAddress = new Uri(Uris.ApiBase);
            return hc;
        }
    }
}
