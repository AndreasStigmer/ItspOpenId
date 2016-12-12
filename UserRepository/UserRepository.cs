using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepository
{
    public class UserRepository:IDisposable
    {

      

        public UserRepository()
        {
            Users = new List<User>();
            loadUsers();
        }
        private List<User> Users { get; set; }
        private StreamReader fileStream { get; set; }

        public void Dispose()
        {
            fileStream.Close();
           
            //throw new NotImplementedException();
        }

        public User GetUser(string username,string password)
        {
            return Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
        }

        //Hämtar en användare baserat på subject
        public User GetUser(string subjectId)
        {
            return Users.FirstOrDefault(u => u.Subject ==subjectId);
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


        private void loadUsers()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Users\users.json";
            fileStream = new StreamReader(path);

            string data = fileStream.ReadToEnd();
            try
            {
                Users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(data);
                if(Users.Count==1)
                {
                    CreateTestUsers();
                }
            }
            catch (Exception e) {
                fileStream.Close();
                CreateTestUsers();
            }
        }

        private void saveUsers()
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Users\users.json";

            using (var outStream = new StreamWriter(path))
            {
                string data=Newtonsoft.Json.JsonConvert.SerializeObject(Users);
                try { 
                    outStream.WriteLine(data);
                }
                finally { 
                    outStream.Flush();
                    outStream.Close();
                }
            }
        }

    }
}
