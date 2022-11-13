using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class Subscription
    {
        public int Id {get;set;}
        public int UserId {get;set;}
        public int FeeId {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get;set;}

        //Foreign Keys
        public User? User {get;set;}
        public Fee? Fee {get;set;}
    }
}