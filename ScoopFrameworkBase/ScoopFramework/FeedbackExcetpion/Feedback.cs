using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ScoopFramework.FeedbackExcetpion
{
    public class Feedback
    {
        public void Success(string msg = "")
        {
            action = "";
            message = msg;
            title = "İşlem Başarılı";
            status = "success";
            timeout = 10;   //sn
            HttpContext.Current.Session["feedback"] = this;
        }

        public void Error(string msg = "İstek işlenirken sorun oluştu. Lütfen tekrar deneyiniz.")
        {
            action = "";
            message = msg;
            title = "Sistem Uyarısı";
            status = "error";
            timeout = 20;   //sn
            HttpContext.Current.Session["feedback"] = this;
        }

        public void Warning(string msg = "")
        {
            action = "";
            message = msg; ;
            title = "İşlem Eksik Gerçekleşti";
            status = "warning";
            timeout = 20;   //sn
            HttpContext.Current.Session["feedback"] = this;
        }

        public string action { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int timeout { get; set; }

    }
}
