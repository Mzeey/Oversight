using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class Setting
    {
        public int Id {get;set;}
        public bool UseTwoFA {get;set;}
        public DateTime DateCreated {get;set;}
        public DateTime DateModified {get;set;}

        //Relations
        public ICollection<UsersSetting> UsersSettings {get;set;}

        public Setting(){
            this.UsersSettings = new HashSet<UsersSetting>();
        }
    }
}