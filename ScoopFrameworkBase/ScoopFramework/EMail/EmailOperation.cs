using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ScoopFramework.EMail
{
   
    public static class EmailOperation
    {
        public static void Send(string email, string baslik, string mesaj)
        {

            Mail().Send(email, baslik, mesaj);

        }

        private static E_MAIL Mail()
        {
            string host = "", username = "", password = "";
            bool ssl = true;
            int port = 0;

            if (System.Configuration.ConfigurationManager.AppSettings["mailHost"] != null)
            {
                host = System.Configuration.ConfigurationManager.AppSettings["mailHost"].ToString();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailPort"] != null)
            {
                port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mailPort"]);
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailUser"] != null)
            {
                username = System.Configuration.ConfigurationManager.AppSettings["mailUser"].ToString();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailPass"] != null)
            {
                password = System.Configuration.ConfigurationManager.AppSettings["mailPass"].ToString();
            }

            if (System.Configuration.ConfigurationManager.AppSettings["mailSsl"] != null)
            {
                ssl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["mailSsl"]);
            }


            var result = new E_MAIL(host, port, username, password, ssl);

            return result;
        }

    }
    public class E_MAIL : IDisposable
    {

        public string hatamesaji = "";
        MailMessage mail = new System.Net.Mail.MailMessage();
        SmtpClient cl = new System.Net.Mail.SmtpClient();
        private string _userName;

        public E_MAIL(string Host, int Port, string userName, string password, bool ssl)
        {
            _userName = userName;
            cl.Host = Host;
            cl.Port = Port;
            cl.EnableSsl = ssl;
            cl.Credentials = new System.Net.NetworkCredential(userName, password);
            mail.IsBodyHtml = false;
            mail.Priority = System.Net.Mail.MailPriority.Normal;
        }
        public bool Send(string email, string baslik, string mesaj, string file = "")
        {
            mail.Body = mesaj;
            mail.To.Add(email);

            mail.From = new System.Net.Mail.MailAddress(_userName, "TARIMSAL İŞLETMELER");
            mail.Subject = baslik;

            if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
            {
                System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(file);
                mail.Attachments.Add(item);
            }
            try
            {

                cl.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                hatamesaji = ex.Message;
            }
            return false;

        }
        public void Dispose()
        {
            mail.Dispose();
        }
    }
}
