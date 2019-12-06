using System.ComponentModel;

namespace ScoopFramework.Enum
{
    public enum DBTable
    {
        Firmalar,
    }

    public enum DBDataTypeEnum
    {
        Int32,
        DBNull,
        String,
        Date,
        DateTime,
        SqlGeography,
        Blooen
    }

    public enum Enum_Gunler
    {

        [Description("Pazartesi")]
        Pazartesi = 1,

        [Description("Salı")]
        Sali = 2,

        [Description("Çarşamba")]
        Carsamba = 3,

        [Description("Perşembe")]
        Persembe = 4,

        [Description("Cuma")]
        Cuma = 5,

        [Description("Cumartesi")]
        Cumartesi = 6,

        [Description("Pazar")]
        Pazar = 7

    }
}
