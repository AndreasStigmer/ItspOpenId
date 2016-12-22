using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepo
{
    class UserRepoDb:DbContext
    {
        public DbSet<UserRepo.User> Users { get; set; }
        public DbSet<UserRepo.UserClaim> UserClaims { get; set; }

    }
}
