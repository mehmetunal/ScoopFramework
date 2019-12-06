using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Model
{
    public class XMLThemesModel
    {
        public ThemeInformation site { get; set; }
        public ThemeInformation Admin { get; set; }
    }

    public class ThemeInformation
    {
        public string Name { get; set; }
        public string ViewPath { get; set; }
        public string ThemeImages { get; set; }
    }
}



///Tema Adı, Tema Path, Tema Resmi
