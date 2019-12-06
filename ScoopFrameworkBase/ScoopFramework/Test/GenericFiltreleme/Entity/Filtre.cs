using System;
using ScoopFramework.Attribute;

namespace ScoopFramework.Test.GenericFiltreleme.Entity
{
   public class Filtre
    {
        public string Donanim { get; set; }
        public string Istasyon { get; set; }

        [RealName("Tarih")]
        [StartDate]
        public DateTime StartDateTime { get; set; }

        [RealName("Tarih")]
        [EndDate]
        public DateTime EndDateTime { get; set; }
        public string RGB { get; set; }
        public string FFT { get; set; }
        public double YesilOrani { get; set; }
        public double BitkiBoyu { get; set; }
        public string BitkiBoyuManuel { get; set; }
        public double Zoom { get; set; }
        public double GunesRadyasyon { get; set; }
        public double Preset { get; set; }
        public string StandartSapma { get; set; }
    }
}
