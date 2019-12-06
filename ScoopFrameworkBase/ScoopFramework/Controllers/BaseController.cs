using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;

namespace ScoopFramework.Controllers
{
    /// <summary>
    /// The base controller.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1005:SingleLineCommentsMustBeginWithSingleSpace", Justification = "Reviewed. Suppression is OK here.")]
    public class BaseController : Controller
    {
        //Here I have created this for execute each time any controller (inherit this) load 

        /// <summary>
        /// The begin execute core.
        /// </summary>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="IAsyncResult"/>.
        /// </returns>
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : string.Empty;
                lang = userLang != string.Empty ? userLang : MultiLanguage.MultiLanguage.GetDefaultlanguage();
                new MultiLanguage.MultiLanguage().SetLanguage(lang);
            }
            return base.BeginExecuteCore(callback, state);
        }

        /// <summary>
        /// The changelanguage.
        /// </summary>
        /// <param name="lang">
        /// The lang.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Changelanguage(string lang)
        {
            new MultiLanguage.MultiLanguage().SetLanguage(lang);
            return Redirect("/Home/Index");
        }
    }
}
