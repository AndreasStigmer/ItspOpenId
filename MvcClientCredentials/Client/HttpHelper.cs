using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace MvcClientCredentials.Client
{
    public class HttpHelper
    {
        public static HttpClient GetClient() {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("http://localhost:51121/");
            hc.DefaultRequestHeaders.Clear();
            hc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var token = GetAccessToken();
            hc.SetBearerToken(token);
            return hc;
        }

        public static string GetAccessToken() {
            TokenClient tc = new TokenClient("https://localhost:44372/identity/connect/token", "MvcClientCredentials","hemligt");
     
            var tokenResult = tc.RequestClientCredentialsAsync("read").Result;
            var token = tokenResult.AccessToken;
            return token;
        }

    }
}
