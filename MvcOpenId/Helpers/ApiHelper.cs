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
            var token = user.FindFirst("access_token").Value;
            hc.DefaultRequestHeaders.Clear();
            hc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            hc.SetBearerToken(token);
            hc.BaseAddress = new Uri(Uris.ApiBase);
            return hc;
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
