using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class StatusMessage
    {

       
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string OwnerId { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class Member
    {
        private List<StatusMessage> _messages;
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<StatusMessage> Messages
        {
            get { 
                if (_messages == null) {
                    _messages = new List<StatusMessage>();
                }
                return _messages;
            }
           set {
                if(value!=null && value.Count>0) { 
                    _messages = value;
                }
            }
        }
    }
}
