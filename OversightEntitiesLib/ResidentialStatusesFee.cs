using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class ResidentialStatusesFee
    {
        public int Id {get;set;}
        public int FeeId {get;set;}
        public int ResidentialStatusId {get;set;}
        public decimal Amount {get;set;}
        public DateTime DateCreated {get;set;}
        public DateTime DateModified {get;set;}

        //Foreign Keys
        public Fee? Fee {get;set;}
        public ResidentialStatus? ResidentialStatus {get;set;}
    }
}