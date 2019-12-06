using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class SecurityClassGenerator
    {

        public Dictionary<string, string> GenerateMultiFile(string solutionName, string databaseName)
        {

            var result = new Dictionary<string, string>();
            result.Add("DBSecurity", CreateSecurityDatabaseFunctions(solutionName, databaseName));
            result.Add("SecurityService", CreateSecurityService(solutionName, databaseName));
            result.Add("SecurityHandler", CreateSecurityHandler(solutionName, databaseName));

            result.Add("DBShell", CreateBDShell(solutionName, databaseName));
            result.Add("PageSecurity", CreatePageSecurity(solutionName, databaseName));
            result.Add("LoginStatus", CreateLoginStatus(solutionName, databaseName));
            return result;
        }

        public string CreateSecurityDatabaseFunctions(string solutionName, string databaseName)
        {
            return string.Format(@"
using {0}.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using {0}.BusinessData;


namespace {0}.BusinessAccess
{{
    partial class {1}Database
    {{
        public SH_User GetSH_UserByTicketid(Guid ticketid)
        {{
            var user = GetSH_TicketById(ticketid);
            if (user != null)
            {{
                using (var db = GetDB())
                {{
                    return db.ExecuteReader<SH_User>(""Select top 1 * From SH_User WITH (NOLOCK)  Where id = {{0}}"", user.userid).FirstOrDefault();
                }}
            }}
            else
                return null;
        }}

        public SH_User GetSHUserById(Guid? id)
        {{
            using (var db = GetDB())
            {{
                return db.ExecuteReader<SH_User>(""Select * From SH_User WITH ( NOLOCK ) Where id = {{0}} ORDER BY Created DESC"", id).FirstOrDefault();
            }}
        }}
        public SH_User GetSH_UserByTckimlikNo(string tckimlikno)
        {{
            using (var db = GetDB())
            {{
                return db.ExecuteReader<SH_User>(""Select * From SH_User WITH (NOLOCK)  Where tckimlikno = {{0}} ORDER BY Created DESC"", tckimlikno).FirstOrDefault();
            }}
        }}
    }}
}}", solutionName, databaseName);
            
        }

        public string CreateSecurityService(string solutionName, string databaseName)
        {
            return string.Format(@"
using {0}.BusinessAccess;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using {0}.BusinessData;

namespace {0}.Business.Security
{{
    [Export(typeof(IService))]
    [ExportMetadata(""ServiceType"", typeof(ISecurityService))]
    public class SecurityService : ISecurityService
    {{
        Timer _tickettimer;
        int _deleteTimeSpan = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
        public SecurityService()
        {{
            _tickettimer = new Timer(DeleteTickets, null, _deleteTimeSpan, Timeout.Infinite);
        }}
        void DeleteTickets(object state)
        {{
            _tickettimer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {{
                using (var db = new {1}Database().GetDB())
                {{
                    db.ExecuteNonQuery(""delete  from SH_Ticket  where endtime < {{0}}"", DateTime.Now);
                }}
            }}
            catch
            {{
            }}
            _tickettimer.Change(_deleteTimeSpan, Timeout.Infinite);
        }}
        public bool ChangePassword(string userid, string password, string newpassword)
        {{
            throw new NotImplementedException();
        }}

        public LoginResult Login(string loginname, string password)
        {{
            var db = new {1}Database();
            var user = db.GetSH_UserByLoginName(loginname);
            if (user != null)
            {{

                if (!string.IsNullOrEmpty(password) && (password == user.password || GetMd5Hash(password) == user.password))
                {{
                    var ticketObject = TicketIsLiveControlByUserid(user.id);
                    if (ticketObject != null)
                    {{
                        var ctx = new CallContext(ticketObject.id, ticketObject.userid, new PropertyBag(), loginname, string.Concat(user.firstname, "" "", user.lastname), new string[0], new PropertyBag(), DateTime.Now.AddDays(1));
                        ctx.Activate();
                    }}
                    else
                    {{
                        var ctx = new CallContext(Guid.NewGuid(), user.id, new PropertyBag(), loginname, string.Concat(user.firstname, "" "", user.lastname), new string[0], new PropertyBag(), DateTime.Now.AddDays(1));
                        ctx.Activate();
                        using (var d = db.GetDB())
                        {{
                            d.ExecuteInsert(new SH_Ticket {{ id = ctx.TicketId, userid = new Guid(ctx.UserId), createtime = DateTime.Now, endtime = DateTime.Now.AddMinutes(TicketLife) }});
                        }}
                    }}
                    return LoginResult.OK;
                }}
            }}
            return LoginResult.InvalidUser;
        }}

        private SH_Ticket TicketIsLiveControlByUserid(Guid userId)
        {{
            var db = new {1}Database();
            using (var d = db.GetDB())
            {{
                var ticketObject = d.ExecuteReader<SH_Ticket>(""Select top 1 * FROM [SH_Ticket] Where userid = {{0}} order by createtime desc"", userId).FirstOrDefault();
                if (ticketObject != null)
                {{
                    if (ticketObject.endtime > DateTime.Now)
                        return ticketObject;
                    else
                        return null;
                }}
                else
                    return null;
            }}
        }}

        public string GetMd5Hash(string input)
        {{
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {{
                sBuilder.Append(data[i].ToString(""x2""));
            }}
            return sBuilder.ToString();
        }}

        public bool IsInRole(string userid, string role)
        {{
            return true;
        }}

        public void SaveTicket(CallContext ctx)
        {{
            using (var db = new {1}Database().GetDB())
            {{
                db.ExecuteInsert(new SH_Ticket {{ id = ctx.TicketId, userid = new Guid(ctx.UserId), endtime = DateTime.Now.AddMinutes(TicketLife) }});
            }}
        }}

        public CallContext LoadTicket(Guid id)
        {{
            var db = new {1}Database();
            using (var d = db.GetDB())
            {{
                var ctx = d.ExecuteReader<SH_Ticket>(""select * from SH_Ticket where  id = {{0}}"", id).FirstOrDefault();
                if (ctx != null)
                {{
                    var user = db.GetSHUserById(ctx.userid);
                    if (user != null)
                    {{
                        return new CallContext(
                            id,
                            ctx.userid,
                            new PropertyBag(), user.loginname, user.firstname, new string[0], new PropertyBag(), Convert.ToDateTime(ctx.endtime));
                    }}
                }}
            }}
            return null;
        }}

        public void DeleteTicket(Guid id)
        {{
            using (var db = new {1}Database().GetDB())
            {{
                db.ExecuteNonQuery(""delete  from SH_Ticket  where id = {{0}}"", id);
            }}
        }}
        public int TicketLife {{ get; set; }}
    }}
}}
", solutionName, databaseName);
        }

        public string CreateSecurityHandler(string solutionName, string databaseName)
        {
            return string.Format(@"

using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using {0}.BusinessAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using {0}.BusinessData;
using Infoline.Framework.Helper;

namespace {0}.Business.Security
{{
    [Export(typeof(ISmartHandler))]
    public class SecurityHandler : BaseSmartHandler
    {{
        public SecurityHandler()
            : base(""security"", new string[] {{ ""cmd"" }})
        {{

        }}

        public override void ProcessRequest(HttpContext context, IDictionary<string, object> paramters)
        {{
            var _start = DateTime.Now;
            var cmd = paramters[""cmd""].ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture);

            var func = typeof(SecurityHandler).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .FirstOrDefault(a => a.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture) == cmd && a.GetParameters().Length == 1);
            if (func != null)
            {{
                func.Invoke(this, new[] {{ context }});
            }}
        }}

        void Logout(HttpContext context)
        {{
            if (CallContext.IsReady)
            {{
                CallContext.Current.Logout();
                RenderResponse(context, new ResultStatus {{ message = ""Successfully"", result = true, objects = null }});
            }}
            else
            {{
                RenderResponse(context, new ResultStatus {{ message = ""User Is Not Found"", result = false, objects = null }});
            }}
        }}
     
        void Login(HttpContext context)
        {{
            var _start = DateTime.Now;
            if (!(CallContext.IsReady))
            {{
                if (HttpContext.Current.Request.HttpMethod == ""POST"")
                {{
                    var loginpost = ParseRequest<Login>(context);

                    if (loginpost != null)
                    {{
                        var userName = new CryptographyHelper().Decrypt(loginpost.username);
                        var passwordBeforeSplit = new CryptographyHelper().Decrypt(loginpost.password).Split('_');
                        var password = passwordBeforeSplit[0];
                        var clientTime = Convert.ToDateTime(passwordBeforeSplit[1]);
                        if (clientTime.Day == DateTime.Now.Day)
                        {{

                            var result = Application.Current.SecurityService.Login(userName, password);
                            ResultStatus status;
                            if (result == Infoline.LoginResult.OK)
                            {{
                                status = new ResultStatus
                                {{
                                    result = true,
                                    objects = new ResultLogin
                                    {{
                                        TicketId = CallContext.Current.TicketId
                                    }},
                                    message = ""TicketId is Lives""
                                }};
                            }}
                            else
                            {{
                                var tResult = String.Empty;

                                if (result == Infoline.LoginResult.InvalidPassword)
                                    tResult = ""Şifreniz kullanıcı adınız ile eşleşmemektedir, lütfen bilgilerinizi kontrol ediniz."";
                                else if (result == Infoline.LoginResult.InvalidUser)
                                    tResult = ""Kullanıcı bulunmamaktadır, lütfen bilgilerinizi kontrol ediniz."";
                                else if (result == Infoline.LoginResult.InvalidUser)
                                    tResult = ""Hesabınız kapatılmıştır. İletişim üzerinden iletişime geçiniz."";
                                else if (result == Infoline.LoginResult.RequiresPasswordChage)
                                    tResult = ""Şifre değişikliğine ihtiyaç vardır. Daha sonra tekrar deneyiniz."";
                                else
                                    tResult = result.ToString();

                                status = new ResultStatus {{ result = false, message = tResult }};
                            }}
                            RenderResponse(context, status);
                        }}
                        else
                        {{
                            context.Response.StatusCode = 401;
                            context.Response.End();
                        }}
                    }}
                    else
                    {{
                        RenderResponse(context, new ResultStatus {{ result = false, objects = null, message = ""Invalid username and password"" }});
                    }}
                }}
                else
                {{
                    RenderResponse(context, new ResultStatus {{ result = false, objects = null, message = ""Method is not POST"" }});
                }}
            }}
            else
            {{
                RenderResponse(context, new ResultStatus
                {{
                    result = true,
                    objects = new ResultLogin
                    {{
                        TicketId = CallContext.Current.TicketId
                    }},
                    message = ""TicketId is Lives""
                }});
            }}
        }}
        void WhoAmI(HttpContext context)
        {{
            try
            {{
                var ticketid = context.Request.Params[""ticketid""];
                if (ticketid != null)
                {{
                    var db = new {1}Database();
                    var user = db.GetSH_UserByTicketid(new Guid(ticketid));
                    RenderResponse(context, user);
                }}
                else
                {{
                    RenderResponse(context, new ResultStatus {{ result = false, message = ""ticketid is null"", objects = null }});
                }}
            }}
            catch (Exception ex)
            {{
                RenderResponse(context, new ResultStatus {{ result = false, message = ex.Message, objects = null }});
            }}
        }}
    }}
}}

public class Login
{{
    public string username {{ get; set; }}
    public string password {{ get; set; }}
}}

public class ResultLogin
{{
    public Guid TicketId {{ get; set; }}
}}
", solutionName, databaseName);
        }

        public string CreateBDShell(string solutionName, string databaseName)
        {
            return string.Format(@"
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Infoline.Framework.Database;
using {0}.BusinessData;

namespace {0}.BusinessAccess
{{
    public partial class {1}Database
    {{
        public SH_User GetUserInfoByUserName(string userName)
        {{
            using (var db = GetDB())
            {{
                return db.ExecuteReader<SH_User>(""Select * From SH_User WITH (NOLOCK)  Where loginname = {{0}}"", userName).FirstOrDefault();
            }}
        }}

        public SH_User GetSH_UserInfoByTcNo(string tckimlikno)
        {{
            using (var db = GetDB())
            {{
                return db.ExecuteReader<SH_User>(""Select * From SH_User WITH (NOLOCK)  Where tckimlikno = {{0}} ORDER BY Created DESC"", tckimlikno).FirstOrDefault();
            }}
        }}

        public SH_User GetSH_UserInfoByEmail(string email)
        {{
            using (var db = GetDB())
            {{
                return db.ExecuteReader<SH_User>(""Select * From SH_User WITH (NOLOCK)  Where email = {{0}} ORDER BY Created DESC"", email).FirstOrDefault();
            }}
        }}

        public SH_User GetSH_UserByLoginName(string loginname)
        {{
            using (var db = GetDB())
            {{
                return db.ExecuteReader<SH_User>(""SELECT * FROM SH_User WITH ( NOLOCK ) WHERE loginname = {{0}} ORDER BY Created DESC"", loginname).FirstOrDefault();
            }}
        }}

        public SH_AuthorityActionRole[] GetSH_AuthorityActionRolesRolId(Guid roleid)
        {{

            using (var db = GetDB())
            {{
                return db.ExecuteReader<SH_AuthorityActionRole>(""Select * From SH_AuthorityActionRole WITH (NOLOCK) where roleid={{0}} ORDER BY created DESC"", roleid).ToArray();
            }}
        }}





        public LoginStatus Login(string userName, string password)
        {{
            var user = GetUserInfoByUserName(userName);

            if (user != null && !user.status)
            {{
                return new LoginStatus {{ LoginResult = LoginResult.AccountDisabled }};
            }}

            var res = new LoginStatus();
            if (user != null)
            {{
                if (!string.IsNullOrEmpty(password) && (password == user.password || GetMd5Hash(password) == user.password))
                {{
                    var ticketid = Guid.NewGuid();
                    using (var d = GetDB())
                    {{
                        
                        var ipAdr = HttpContext.Current.Request.ServerVariables[""REMOTE_ADDR""];

                        d.ExecuteInsert(new SH_Ticket
                        {{
                            id = ticketid,
                            userid = user.id,
                            createtime = DateTime.Now,
                            endtime = DateTime.Now.AddMinutes(30),
                            IP = ipAdr
                        }});
                    }}
                    res.LoginResult = LoginResult.OK;
                    res.ticketid = ticketid;
                    return res;
                }}
                else
                {{
                    res.LoginResult = LoginResult.InvalidPassword;
                }}
            }}
            else
                res.LoginResult = LoginResult.InvalidUser;

            return res;
        }}

        public string GetMd5Hash(string input)
        {{
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {{
                sBuilder.Append(data[i].ToString(""x2""));
            }}
            return sBuilder.ToString();
        }}

        public PageSecurity GetUserPageSecurityByticketid(Guid ticketid)
        {{
            if (ticketid != null)
            {{
                var pageSecurity = new PageSecurity();
                using (var db = GetDB())
                {{
                    var sh_Ticket = db.ExecuteReader<SH_Ticket>(""SELECT TOP 1 * FROM SH_Ticket WITH (NOLOCK) WHERE id = {{0}} order by createtime desc"", ticketid).FirstOrDefault();
                    var sh_UserRoles = db.ExecuteReader<SH_UserRole>(""SELECT * From SH_UserRole WITH (NOLOCK) WHERE userid = {{0}} AND status = 1 order by created desc"", sh_Ticket.userid).ToArray();
                    var sh_User = db.ExecuteReader<SH_User>(""SELECT * FROM SH_User WITH (NOLOCK) WHERE id = {{0}} AND status = 1 order by created desc"", sh_Ticket.userid).FirstOrDefault();
                    if (sh_UserRoles.Count() != 0)
                    {{
                        var roleId = string.Join("","", sh_UserRoles.Select(role => string.Format(""'{{0}}'"", role.roleid)));
                        var authorityActionRole = db.ExecuteReader<SH_AuthorityActionRole>(""Select * From SH_AuthorityActionRole WITH (NOLOCK) Where roleid IN("" + roleId.ToString() + "") AND status = 1 order by created desc"").ToArray();  
                        if (sh_User.type != null) {{ pageSecurity.type = Convert.ToInt32(sh_User.type); }}
                        if (authorityActionRole.Count() > 0) {{ pageSecurity.AuthorityActionRoles = authorityActionRole; }}
                        if (sh_UserRoles != null && sh_UserRoles.Count() > 0) {{ pageSecurity.Roles = sh_UserRoles.Select(a => (Guid)a.roleid).ToArray(); }}
                    }}
                    pageSecurity.user = sh_User;
                    pageSecurity.ticketid = ticketid;
                    pageSecurity.UserRoles = sh_UserRoles;

                    return pageSecurity;
                }}
            }}
            return null;
        }}
    }}
}}", solutionName, databaseName);
        }
        public string CreatePageSecurity(string solutionName, string databaseName)
        {
            return string.Format(@"
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Infoline.Framework.Database;
using {0}.BusinessData;

namespace {0}.BusinessAccess
{{

    public class PageSecurity
    {{
        public Guid ticketid {{ get; set; }}
        public Guid[] Roles {{ get; set; }}
        public SH_User user {{ get; set; }}
        public SH_UserRole[] UserRoles {{ get; set; }}
        public SH_AuthorityActionRole[] AuthorityActionRoles {{ get; set; }}
        public int type {{ get; set; }}
    }}

}}", solutionName);
        }
        public string CreateLoginStatus(string solutionName, string databaseName)
        {
            return string.Format(@"
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Infoline.Framework.Database;
using {0}.BusinessData;

namespace {0}.BusinessAccess
{{

    public class LoginStatus
    {{
        public LoginResult LoginResult {{ get; set; }}
        public Guid ticketid {{ get; set; }}

    }}

}}", solutionName);
        }

    }
}
