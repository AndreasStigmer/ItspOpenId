using IdentityModel.Client;
using Model;
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
        /// <summary>
        /// Skapar en HTTP client för att kommunicera med apiet.
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetClient() {
            HttpClient hc = new HttpClient();
            ClaimsIdentity user = HttpContext.Current.User.Identity as ClaimsIdentity;
            var token = GetToken();
            hc.DefaultRequestHeaders.Clear();
            hc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            hc.SetBearerToken(token);
            hc.BaseAddress = new Uri(Uris.ApiBase);
            return hc;
        }

        public static string GetToken()
        {
            ClaimsIdentity user = HttpContext.Current.User.Identity as ClaimsIdentity;
            var expires = DateTime.Parse(user.FindFirst("expire").Value);

            if(expires>DateTime.UtcNow)
            {
                return user.FindFirst("access_token").Value;
            }

            TokenClient tc = new TokenClient(Uris.STSTokenEndpoint, "MvcOpenId", "hemligt");
            var tresponse = tc.RequestRefreshTokenAsync(user.FindFirst("refresh_token").Value).Result;

            if(!tresponse.IsError)
            {
                var claims = user.Claims.Where(c => c.Type != "access_token"  && c.Type != "refresh_token" && c.Type != "expire").ToList();
                ClaimsIdentity newIdent = new ClaimsIdentity(claims, "cookie");
                var expire = DateTime.UtcNow.AddSeconds(tresponse.ExpiresIn).ToString();
                newIdent.AddClaim(new Claim("expire", expire));
                newIdent.AddClaim(new Claim("access_token",tresponse.AccessToken));
                //newIdent.AddClaim(new Claim("id_token", tresponse.IdentityToken));
                newIdent.AddClaim(new Claim("refresh_token", tresponse.RefreshToken));

                HttpContext.Current.Request.GetOwinContext().Authentication.SignIn(newIdent);

                return tresponse.AccessToken; 
            }

            return "";

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
