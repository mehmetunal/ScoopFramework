using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopFramework.Entity
{
    public class SCP_Log
    {
        public Guid id { get; set; }
        public DateTime createddate { get; set; }
        public Guid createdby { get; set; }
        public Guid LogTableId { get; set; }
        public string Url { get; set; }
        public int Method { get; set; }
        public string Baslik { get; set; }
        public string Mesaj { get; set; }
        public string Detay { get; set; }
        public int Durum { get; set; }
    }
}
