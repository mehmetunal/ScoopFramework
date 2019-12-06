using System;
using System.Web;

namespace ScoopFramework.Model
{
    public  class ReturnJson
    {
        public bool Result { get; set; }
        public object Object { get; set; }
        public FeedBack FeedBack { get; set; }
    }

    public class SiteMap
    {
        public string Url { get; set; }
    }
    public class Rss
    {
        public int RssId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string PubDate { get; set; }
    }
    public class FeedBack
    {

        public FeedBack Success(string msg = "", bool sessionCreate = false, string action = null)
        {

            var result = new FeedBack
            {
                action = action ?? String.Empty,
                message = msg,
                title = "İşlem Başarılı",
                status = "success",
                timeout = 10, //  saniye
            };

            if (sessionCreate)
            {
                HttpContext.Current.Session["feedback"] = result;
            }

            return result;
        }



        public FeedBack Error(string logMessage, string msg = "İstek işlenirken sorun oluştu. Lütfen tekrar deneyiniz.", bool sessionCreate = false)
        {

            //Log.Error(logMessage);

            var result = new FeedBack
            {
                action = "",
                message = msg,
                title = "Sistem Uyarısı",
                status = "error",
                timeout = 20, //  saniye
            };

            if (sessionCreate)
            {
                HttpContext.Current.Session["feedback"] = result;
            }

            return result;

        }


        public FeedBack Warning(string msg = "", bool sessionCreate = false)
        {
            var result = new FeedBack
            {
                action = "",
                message = msg,
                title = "İşlem Eksik Gerçekleşti",
                status = "warning",
                timeout = 20, //  saniye
            };

            if (sessionCreate)
            {
                HttpContext.Current.Session["feedback"] = result;
            }

            return result;
        }

        public FeedBack Custom(string msg = "", string action = "", string title = "Bilgilendirme", string status = "success", int timeout = 10, bool sessionCreate = false)
        {
            var result = new FeedBack
            {
                action = action,
                message = msg,
                title = title,
                status = status,
                timeout = timeout, //  saniye
            };

            if (sessionCreate)
            {
                HttpContext.Current.Session["feedback"] = result;
            }

            return result;
        }

        public string action { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int timeout { get; set; }
    }
}
