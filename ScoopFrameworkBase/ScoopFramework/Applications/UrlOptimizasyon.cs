namespace ScoopFramework.Applications
{
    public static class UrlOptimizasyon
    {
        public static string Url(string query)
        {
            string strReturn = query;
            strReturn = strReturn.Replace("ğ", "g");
            strReturn = strReturn.Replace("Ğ", "g");
            strReturn = strReturn.Replace("ü", "u");
            strReturn = strReturn.Replace("Ü", "u");
            strReturn = strReturn.Replace("ş", "s");
            strReturn = strReturn.Replace("Ş", "s");
            strReturn = strReturn.Replace("ı", "i");
            strReturn = strReturn.Replace("İ", "i");
            strReturn = strReturn.Replace("ö", "o");
            strReturn = strReturn.Replace("Ö", "o");
            strReturn = strReturn.Replace("ç", "c");
            strReturn = strReturn.Replace("Ç", "c");
            strReturn = strReturn.Replace("-", "+");
            strReturn = strReturn.Replace(" ", "+");
            strReturn = strReturn.Trim();
            strReturn = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9+]").Replace(strReturn, "");
            strReturn = strReturn.Trim();
            strReturn = strReturn.Replace("+", "-").Replace('ş', 's')
                .Replace('Ş', 'S')
                .Replace('ç', 'c')
                .Replace("Ç", "c")
                .Replace('ğ', 'g')
                .Replace('Ğ', 'g')
                .
                Replace('ü', 'u')
                .Replace('Ü', 'u')
                .Replace('ı', 'i')
                .Replace('İ', 'i')
                .Replace('ö', 'o')
                .Replace('Ö', 'O')
                .Replace("?", "")
                .Replace(",", "")
                .Replace("/", "")
                .Replace(".", "")
                .Replace("\"", "");

            return strReturn;
        }
    }
}
