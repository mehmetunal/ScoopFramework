using System.Collections.Generic;

namespace ScoopFramework.Helper
{
    public static class DLLHelper
    {
        private static Dictionary<string, string>[] _dllPath =
        {
            new Dictionary<string, string>() {{"Solition", "Path1"}},
            new Dictionary<string, string>() {{"Solition", "Path2"}},
            new Dictionary<string, string>() {{"Solition", "Path3"}}
        };
        public static Dictionary<string, string>[] DllPath => _dllPath;
    }
}
