using IdentityModel.Client;
using Model;
using ProjConstants;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        /// <summary>
        /// Skapar en HTTP client för att kommunicera med apiet.
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetClient() {
            HttpClient hc = new HttpClient();
            var token = GetAccessToken();
            hc.DefaultRequestHeaders.Clear();
            hc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            hc.SetBearerToken(token);
            hc.BaseAddress = new Uri(Uris.ApiBase);
            return hc;
        }

        public static string GetAccessToken() {
            ClaimsIdentity user = HttpContext.Current.User.Identity as ClaimsIdentity;

            var expire = DateTime.Parse(user.FindFirst("expires").Value);
            var now = DateTime.UtcNow;
            if (now<expire)
            {
                return user.FindFirst("access_token").Value;
            }
            TokenClient tc = new TokenClient(Uris.STSTokenEndpoint, "MvcOpenId", "hemligt");
            var response = tc.RequestRefreshTokenAsync(user.FindFirst("refresh_token").Value).Result;

            if (!response.IsError)
            {
                var expiresAt = DateTime.UtcNow.AddSeconds(response.ExpiresIn).ToString();
                var claims = user.Claims.Where(c => c.Type != "access_token" && c.Type != "refresh_token" && c.Type != "expires").ToList();
                claims.Add(new Claim("access_token", response.AccessToken));
                claims.Add(new Claim("refresh_token", response.RefreshToken));
                claims.Add(new Claim("expires", expiresAt));
                ClaimsIdentity ci = new ClaimsIdentity(claims, "cookie", "name", "role");
                HttpContext.Current.Request.GetOwinContext().Authentication.SignIn(ci);
                return response.AccessToken;
            }
            else {
                HttpContext.Current.Request.GetOwinContext().Authentication.SignOut();
                return "";
            }

          }

        /// <summary>
        /// Sparar en member över apiet
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public async Task<string> SaveMember(Member m) {
            HttpClient hc = GetClient();
            string member = Newtonsoft.Json.JsonConvert.SerializeObject(m);
            StringContent sc = new StringContent(member, Encoding.UTF8, "application/json");
            var response = await hc.PutAsync("/api/Member", sc);
            var responsecode= await response.Content.ReadAsStringAsync();
            return responsecode;
        }


        /// <summary>
        /// Hämtar en medlem över apiet baserat på medlemsid/name claim
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns></returns>
        public async Task<Member> GetMember(string memberid)
        {
            HttpClient hc = ApiHelper.GetClient();
            string uri = "/api/member/?id=" + HttpUtility.UrlEncode(memberid);
            var value = await hc.GetStringAsync(uri);
            Member member= Newtonsoft.Json.JsonConvert.DeserializeObject<Member>(value);
            return member;

        }
    }
}
