using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class PaymentMethod
    {
        public int Id {get;set;}
        public int UserId {get;set;}
        public string? Title {get;set;}
        public string? CardHolderName { get;set;}
        public string? CardNumber {get;set;}
        public string? CardType {get;set;}
        public DateTime DateCreated {get;set;}


        //Foreign Key
        public User? User {get;set;}
    }
}