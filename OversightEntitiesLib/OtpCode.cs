using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class OtpCode
    {
        public int Id {get;set;}
        public int UserId {get;set;}
        /// <summary>
        /// It contains a temporarily valid six digit code
        /// </summary>
        /// <value>six digit code</value>
        public int Code {get;set;}
        public bool isValid {get;set;}
        public DateTime DateCreated {get;set;}
        public DateTime ExpiryDate {get;set;}

        //Foreign Key
        public User? User {get;set;}

        public OtpCode(){
            this.User = new User();
        }
    }
}