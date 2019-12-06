using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScoopFramework.UrlController
{
    class UrlSeoController
    {
        public string GenerateSlug(object name)
        {
            var phrase = string.Format("{0}", name);
            var str = RemoveAccent(phrase).ToLower();
            str = RemoveAccent(str).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
