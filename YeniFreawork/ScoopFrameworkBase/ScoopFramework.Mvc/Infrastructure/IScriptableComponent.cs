using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Mvc.Infrastructure
{
    public interface IScriptableComponent
    {
        bool IsSelfInitialized
        {
            get;
        }

        bool IsInClientTemplate
        {
            get;
        }

        string Selector
        {
            get;
        }

        /// <summary>
        /// Writes the initialization script.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void WriteInitializationScript(TextWriter writer);
    }
}
