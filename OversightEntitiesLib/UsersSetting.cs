using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Shared
{
    public class UsersSetting
    {
        public int Id {get;set;}
        public int UserId {get;set;}
        public int SettingId {get;set;}

        //Foreign Keys
        public User? User {get;set;}
        public Setting? Setting {get;set;}
    }
}