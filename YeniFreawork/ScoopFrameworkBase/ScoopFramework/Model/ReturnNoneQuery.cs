using System.ComponentModel;

namespace ScoopFramework.Model
{
    public class ReturnNoneQuery
    {
        public enum EnumExecuteNonQuery
        {
            [Description("Başarılı")]
            basarili=1,
            [Description("Başarısız")]
            hatali =0,
        }
    }
}
