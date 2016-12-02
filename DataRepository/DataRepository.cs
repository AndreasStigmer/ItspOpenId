using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Model;

namespace DataRepository
{
    public class MemberRepository
    {
        private List<Member> _users;

        public MemberRepository()
        {
            _users = new List<Member>();
        }
        
        public List<Member> GetUsers(){
            if(_users==null || _users.Count==0)
            {
                _users = FileHandler.LoadUsers();
            }
            return _users;
        }


        public bool SaveUser(Member su)
        {
            FileHandler.SaveUser(su);
            _users.Add(su);
            return true;
        }
    }
}
