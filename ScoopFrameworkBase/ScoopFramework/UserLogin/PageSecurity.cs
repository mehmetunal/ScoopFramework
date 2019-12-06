using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoopFramework.DBModel;

namespace ScoopFramework.UserLogin
{
    public class PageSecurity
    {
        public int userid { get; set; }
        public int hastaneId { get; set; }
        public int firmaid { get; set; }
        public List<int?> Roles { get; set; }
        //public List<SH_UserRole> UserRoles { get; set; }
        //public List<SHAuthorityActionRole> AuthorityActionRoles { get; set; }
        public string userName { get; set; }
        public string firmaName { get; set; }
    }
    //public class SHAuthorityActionRole : SH_AuthorityActionRole
    //{
    //    public string actionName { get; set; }
    //    public string statusName { get; set; }
    //    public string pageTitle { get; set; }
    //    public string yetkiName { get; set; }
    //}
}
