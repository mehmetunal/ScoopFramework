namespace Scoop.Captcha
{
    public static class CaptchaSucces
    {
        public static bool Succes(string param)
        {
            if (System.Web.HttpContext.Current.Session["code"] != null && !string.IsNullOrEmpty(param))
            {
                param = param.Trim();
                var code = System.Web.HttpContext.Current.Session["code"].ToString().Trim();
                if (code.Equals(param))
                {
                    System.Web.HttpContext.Current.Session.Remove("code");
                    return true;
                }
            }
            System.Web.HttpContext.Current.Session.Remove("code");
            return false;
        }
    }
}
