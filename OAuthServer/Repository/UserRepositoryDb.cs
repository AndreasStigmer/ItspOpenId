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

        public  void Dispose() {
            db = null;
        }
    }
}
