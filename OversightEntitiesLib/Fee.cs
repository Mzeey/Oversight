using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class Fee
    {
        public int Id {get;set;}
        public string? Title {get;set;}
        public bool IsActive {get;set;}
        public DateTime DateCreated {get;set;}
        public DateTime DateModified {get;set;}

        //Relations
        public ICollection<ResidentialStatusesFee>? ResidentStatusesFees {get;set;}
        public ICollection<Subscription>? Subscriptions {get;set;}
        public Fee(){
            this.ResidentStatusesFees = new HashSet<ResidentialStatusesFee>();
            this.Subscriptions = new HashSet<Subscription>();
        }
    }
}