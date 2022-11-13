using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class TwoFA
    {

        public int Id {get;set;}
        public int UserId {get;set;}

        /// <summary>
        /// This is either email or phone
        /// </summary>
        /// <value>"email" or "phone"</value>
        public string? AuthReceiver {get;set;}
        public DateTime DateCreated {get;set;}
        public DateTime DateModified {get;set;}

        //Foreign Keys
        public User? User {get;set;}
    }
}