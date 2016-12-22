using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepo
{
    public class UserRepository : IDisposable
    {

        private List<User> Users { get; set; }
        private StreamReader fileStream { get; set; }


        public UserRepository()
        {
            Users = new List<User>();
            loadUsers();
        }

        public void Dispose()
        {
            fileStream.Close();
            //throw new NotImplementedException();
        }

        //Hämtar en användare baserat på användarnamn och lösenord
        public User GetUser(string username, string password)
        {
            return Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
        }

        //Hämtar en användare baserat på subject
        public User GetUser(string subjectId)
        {
            return Users.FirstOrDefault(u => u.Subject == subjectId);
        }


        private void CreateTestUsers()
        {

            User u = new User();
            u.Subject = "098432098432098";
            u.IsActive = true;
            u.Password = "hemligt";
            u.UserName = "andreas";
            u.UserClaims.Add(new UserClaim { ClaimType = "given_name", ClaimValue = "Andreas" });
            u.UserClaims.Add(new UserClaim { ClaimType = "family_name", ClaimValue = "Stigmer" });
            u.UserClaims.Add(new UserClaim { ClaimType = "email", ClaimValue = "andreas@campusi12.se" });
            u.UserClaims.Add(new UserClaim { ClaimType = "role", ClaimValue = "admin" });
            Users.Add(u);

            saveUsers();
        }


        /// <summary>
        /// Läser in alla Users från jsonfilen
        /// </summary>
        private void loadUsers()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Users\users.json";
            fileStream = new StreamReader(path);

            string data = fileStream.ReadToEnd();
            try
            {
                Users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(data);
                if (Users.Count == 1)
                {
                    fileStream.Close();
                    CreateTestUsers();
                }
            }
            finally
            {
                fileStream.Close();
               
            }
        }

        //Skriver ner Listan med users till en Jsonfil
        private void saveUsers()
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + @"Users\users.json";

            using (var outStream = new StreamWriter(path))
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(Users);
                try
                {
                    outStream.WriteLine(data);
                }
                finally
                {
                    outStream.Flush();
                    outStream.Close();
                }
            }
        }

        public void AddUser(User newUser)
        {
            this.Users.Add(newUser);
            saveUsers();
        }

    }
}
