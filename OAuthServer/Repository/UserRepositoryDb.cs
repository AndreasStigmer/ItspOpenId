using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepo
{
    public class UserRepositoryDb :IDisposable
    {
        UserRepoDb db;
        public UserRepositoryDb()
        {
            db = new UserRepoDb();
        }
        public void AddUser(UserRepo.User newUser) {
            db.Users.Add(newUser);
            db.SaveChanges();
        }

        public UserRepo.User GetUser(string username, string password) {
            return db.Users.FirstOrDefault(d => d.UserName == username && d.Password == password);
        }

        public UserRepo.User GetUser(string subject) {
            return db.Users.FirstOrDefault(d => d.Subject == subject);
        }

        public UserRepo.User GetUserForExternalUser(string providerkey,string provider) {
            var user = db.Users.FirstOrDefault(d => d.UserLogins.Any(e => e.ProviderKey == providerkey && e.LoginProvider == provider));
            return user;
        }

        public void AddLoginToUser(User user, UserLogin ul) {
            user.UserLogins.Add(ul);
            db.SaveChanges();
        }

        public UserRepo.User GetUserByEmail(string email)
        {
            var user = db.Users.FirstOrDefault(d => d.UserClaims.Any(c=>c.ClaimType=="email" && c.ClaimValue==email));
            return user;
        }

        public  void Dispose() {
            db = null;
        }
    }
}
