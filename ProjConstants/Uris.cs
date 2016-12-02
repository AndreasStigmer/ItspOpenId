using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjConstants
{
    public class Uris
    {
        public const string IssuerURI = "https://MyISSUER:44372";
        public const string STS = "https://localhost:44372/identity";
        public const string STSTokenEndpoint = STS+"/connect/token";
        public const string STSAuthorizeEndpoint = STS + "/connect/authorize";
        public const string STSUserInfoEndpoint = STS + "/connect/userinfo";
        public const string MvcAuthCodeCallback = "http://localhost:63534/STSCallback";
        public const string MvcOpenIdCallback = "http://localhost:50621/";

        public const string ApiBase = "http://localhost:51121/";


    }
}
