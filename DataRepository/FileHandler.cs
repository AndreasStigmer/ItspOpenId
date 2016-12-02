using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public class FileHandler
    {
        private static string DataPath { get {
                return AppDomain.CurrentDomain.BaseDirectory + "\\data\\Data.json";
            }
        }

        /// <summary>
        /// Laddar in alla users och deras messages
        /// </summary>
        /// <returns></returns>
        public static List<Member> LoadUsers()
        {
            string data = "";
            using (StreamReader sr = new StreamReader(DataPath)) { 
                data = sr.ReadToEnd();
            }
            List<Member> su = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Member>>(data);
            return su;
        }


        /// <summary>
        /// Sparar ny elle ruppdaterar en user i jsonfilen
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool SaveUser(Member user)
        {
            List<Member> su = LoadUsers();
            Member found = su.FirstOrDefault(d => d.UserId == user.UserId);
            if(found==null) {
                su.Add(user);
            }else
            {
                found.LastName = user.LastName;
                found.FirstName = user.FirstName;
                found.Messages = user.Messages;
            }

            using (StreamWriter sw = new StreamWriter(DataPath, false)) { 
                var jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(su);
                sw.WriteLine(jsonstring);
                sw.Flush();
            }
            return true;
        }


    }
}
