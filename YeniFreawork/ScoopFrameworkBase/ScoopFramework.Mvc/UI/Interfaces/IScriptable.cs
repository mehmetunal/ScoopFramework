using System.IO;

namespace ScoopFramework.Mvc
{
    public interface IScriptable
    {
        void WriteInitializationScript(TextWriter writer);
    }
}