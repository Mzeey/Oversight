using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class Invoice
    {
        public int Id {get;set;}
        public int UserId {get;set;}
        public decimal Amount {get;set;}
        public int DurationInMonths {get;set;}
        public DateTime DatePaid {get;set;}
        public string? ModeOfPayment {get;set;}

        //Foreign Key
        public User User {get;set;}

        public Invoice(){
            this.User = new User();
        }
    }
}