using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using ScoopFramework.Model;

namespace ScoopFramework.MultiLanguage
{
    /// <summary>
    /// The multi language.
    /// </summary>
    public class MultiLanguage
    {
        /// <summary>
        /// The available languageses.
        /// </summary>
        private static List<Languages> AvailableLanguageses = new List<Languages>
        {
            new Languages{LangFullName = "Türkçe",LangCulturename = "tr"},
            new Languages{LangFullName = "English",LangCulturename = "en"}
        };

        /// <summary>
        /// The is language available.
        /// </summary>
        /// <param name="lang">
        /// The lang.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsLanguageAvailable(string lang)
        {
            return AvailableLanguageses.Where(a => a.LangCulturename.Equals(lang)).FirstOrDefault() != null ? true : false;
        }

        /// <summary>
        /// The get defaultlanguage.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDefaultlanguage()
        {
            var defaultlanguage = "tr";
            if (HttpContext.Current.Request.Cookies.Count > 0)
            {
                defaultlanguage = HttpContext.Current.Request.Cookies.Get("culture").Value;
            }
            int lng = 0;
            switch (defaultlanguage)
            {
                case "tr": lng = 0;
                    break;
                case "en": lng = 1;
                    break;
            }
            return lng.ToString();
        }

        /// <summary>
        /// The set language.
        /// </summary>
        /// <param name="lang">
        /// The lang.
        /// </param>
        public void SetLanguage(string lang)
        {
            try
            {
                if (!IsLanguageAvailable(lang))
                {
                    lang = GetDefaultlanguage();
                }
                //hata var
                var lng = lang == "0" ? "tr" : "en";
                var cultureInfo = new CultureInfo(lng);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                var langCookie = new HttpCookie("culture", lang) { Expires = DateTime.Now.AddYears(1) };
                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
            catch (Exception ex)
            {

                throw;
            }
        }



    }
}
