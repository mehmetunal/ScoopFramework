using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Themes
{
    public interface IThemeContext
    {
        /// <summary>
        /// Get or set current theme system name
        /// </summary>
        string WorkingThemeName { get; set; }
    }
}
