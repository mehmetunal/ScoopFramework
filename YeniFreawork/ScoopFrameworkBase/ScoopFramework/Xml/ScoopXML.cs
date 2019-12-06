using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Microsoft.SqlServer.Management.Smo;
using ScoopFramework.Model;
using ScoopFramework.Themes;

namespace ScoopFramework.Xml
{
    public sealed class ScoopXML
    {
        public static XmlNodeList ScoopXMLRead(string xmlPath, string xmlChild)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(xmlPath));
            return doc.SelectNodes(xmlChild);
        }

        public static FileInfo ThemeImagesPath(string path)
        {
            var themeDirectory = new DirectoryInfo(path);
            return new FileInfo(Path.Combine(themeDirectory.FullName, "preview.jpg"));
        }

        public static XMLThemesModel[] GetProjectThemes(string xmlPath)
        {
            var theme = new List<XMLThemesModel>();

            foreach (XmlNode node in ScoopXMLRead(xmlPath, "/themes"))
            {
                var adminFullName = HttpContext.Current.Request.MapPath("/" + node["Admin"].InnerText);
                var siteFullName = HttpContext.Current.Request.MapPath("/" + node["site"].InnerText);


                var findFolderPath = string.Format(@"{0}", HttpContext.Current.Request.PhysicalApplicationPath);

                string[] dirs = Directory.GetFiles(findFolderPath, node["site"].InnerText+"*");


                theme.Add(new XMLThemesModel
                {
                    Admin = new ThemeInformation()
                    {
                        Name = node["Admin"].InnerText,
                        ViewPath = adminFullName,
                        ThemeImages = ThemeImagesPath(adminFullName).FullName
                    },
                    site = new ThemeInformation()
                    {
                        Name = node["site"].InnerText,
                        ViewPath = siteFullName,
                        ThemeImages = ThemeImagesPath(siteFullName).FullName
                    },
                });
            }
            return theme.ToArray();
        }

    }
}
