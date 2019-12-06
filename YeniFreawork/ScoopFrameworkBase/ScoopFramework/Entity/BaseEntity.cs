using System;
using System.ComponentModel;

namespace ScoopFramework.Entity
{
    public partial class BaseEntity
    {
        public BaseEntity()
        {
            id = Guid.NewGuid();
        }

        public Guid id { get; set; }

        [Description("Kayıt Tarihi")]
        public DateTime? created { get; set; }

        [Description("Güncelleme Tarihi")]
        public DateTime? changed { get; set; }

        [Description("Kayıt Atan Kişi")]
        public Guid? changedby { get; set; }

        [Description("Güncelleyen Kişi")]
        public Guid? createdby { get; set; }
    }
}
