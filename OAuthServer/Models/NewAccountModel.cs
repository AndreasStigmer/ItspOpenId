using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthServer.Models
{
    public class NewAccountModel
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public string  FirstName { get; set; }
        public string LastName { get; set; }
    }
}
