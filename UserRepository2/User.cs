using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepo
    {
        public class User
        {
            public User()
            {
                UserClaims = new List<UserClaim>();
                UserLogins = new List<UserLogin>();
            }
            public string Subject { get; set; }

            public string UserName { get; set; }
            public string Password { get; set; }

            public bool IsActive { get; set; }

            public IList<UserClaim> UserClaims { get; set; }
            public IList<UserLogin> UserLogins { get; set; }

        }

        public class UserClaim
        {
            public string Id { get; set; }
            public string Subject { get; set; }

            public string ClaimType { get; set; }

            public string ClaimValue { get; set; }
        }

        public class UserContext
        {

        }

        public class UserLogin
        {

        }
    }


