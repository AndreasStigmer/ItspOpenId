using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserRepo
    {
        public class User
        {
            public User()
            {
                UserClaims = new List<UserClaim>();
               // UserLogins = new List<UserLogin>();
            }
            [Key]
            public string Subject { get; set; }
            
            public string UserName { get; set; }
            public string Password { get; set; }

            public bool IsActive { get; set; }

            public virtual IList<UserClaim> UserClaims { get; set; }
            public IList<UserLogin> UserLogins { get; set; }

        }

        public class UserClaim
        {
            [Key]
            public string Id { get; set; }

            [ForeignKey("Subject")]
            public User Owner { get; set; }

            
            public string Subject { get; set; }

            public string ClaimType { get; set; }

            public string ClaimValue { get; set; }
        }

        public class UserContext
        {

        }

        public class UserLogin
        {
            [Key]
            public int ID { get; set; }
            public string ProviderKey { get; set; }
            public string Subject { get; set; }

            public string LoginProvider { get; set; }

        }
    }


