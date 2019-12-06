using ScoopFramework.Entity;
using System;
using System.ComponentModel;

namespace ScoopFramework.DataBussiens
{
    /// <summary>
    /// Represents a SCP_Log.
    /// NOTE: This class is generated from a T4 template - you should not modify it manually.
    /// </summary>
    public class SCP_Log : BaseEntity
    {
        public DateTime? createddate { get; set; }

        public Guid? LogTableId { get; set; }

        public string Url { get; set; }

        public int? Method { get; set; }

        public string Baslik { get; set; }

        public string Mesaj { get; set; }

        public string Detay { get; set; }

        public int? Durum { get; set; }
    }
}
