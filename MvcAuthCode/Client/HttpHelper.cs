using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ProjConstants;
using System.Net.Http.Headers;
using System.Web;
using IdentityModel.Client;

namespace MvcAuthCode.Client
{
    public class HttpHelper
    {

        public static HttpClient GetClient() {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri(Uris.ApiBase);
            hc.DefaultRequestHeaders.Clear();
            hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var token = GetAuthCode();
            hc.SetBearerToken(token);

            return hc;
            
        }

        public static string GetAuthCode() {
            var cookie = HttpContext.Current.Request.Cookies["mycookie"];
            if (cookie != null && cookie["access_token"] != null)
            {
                return cookie["access_token"];
                
            }

            AuthorizeRequest ar = new AuthorizeRequest(Uris.STSAuthorizeEndpoint);
            var state = HttpContext.Current.Request.Url.OriginalString;
            string uri = ar.CreateAuthorizeUrl("MvcAuthCode", "code", "read", Uris.MvcAuthCodeCallback, state);

            HttpContext.Current.Response.Redirect(uri);
            return null;
        }
    }
}
