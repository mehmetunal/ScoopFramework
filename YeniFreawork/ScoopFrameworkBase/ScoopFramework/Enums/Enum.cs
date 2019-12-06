using System.ComponentModel;
namespace ScoopFramework.Enums
{
    class Enum { }

    public enum EnumTraceLogProviderMethod
    {
        [Description("İnsert")]
        Insert = 0,
        [Description("Update")]
        Update = 1,
        [Description("Delete")]
        Delete = 2
    }

    public enum EnumTraceLogProviderDurum
    {
        [Description("Success")]
        Success=0,
        [Description("Info")]
        Info = 1,
        [Description("Warning")]
        Warning = 2,
        [Description("Danger")]
        Danger = 3
    }
}
