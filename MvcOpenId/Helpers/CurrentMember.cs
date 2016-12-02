using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MvcOpenId.Helpers
{
    public class CurrentMember
    {

        public static async Task<Member> Get()
        {
            ClaimsIdentity ci=HttpContext.Current.User.Identity as ClaimsIdentity;
            HttpClient hc=ApiHelper.GetClient();
            string uri = "/api/member/?id="+  HttpUtility.UrlEncode (ci.Name);

            var value=await hc.GetStringAsync(uri);
            return  Newtonsoft.Json.JsonConvert.DeserializeObject<Member>(value);
        }

        public static async Task<Member> Register(ClaimsIdentity identity)
        {
            Member newMember = new Member();
            newMember.FirstName = identity.FindFirst("given_name").Value;
            newMember.LastName= identity.FindFirst("family_name").Value;
            newMember.UserId = identity.Name;

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(newMember) ;
            var stringContent = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            HttpClient hc = ApiHelper.GetClient();
            var response=await hc.PostAsync("api/member",stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(result);
            Member created = await Get();
            return created;
        }

    }
}
