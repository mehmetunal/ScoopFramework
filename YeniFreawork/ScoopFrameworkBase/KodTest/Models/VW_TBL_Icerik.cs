using System;

namespace KodTest.Models
{
    public class famous_people
    {
        public int id { get; set; }

        public string peo_name { get; set; }

        public string peo_about { get; set; }

        public string peo_title { get; set; }

        public string peo_jpg { get; set; }

        public string link { get; set; }

        public string peo_birthplace { get; set; }

        public DateTime? peo_birthday { get; set; }

        public DateTime? deathhday { get; set; }

        public string yearslink { get; set; }

        public string joplink { get; set; }

        public string peo_birthplacelink { get; set; }
    }
    public class VW_TBL_Icerik
    {
        public Guid id { get; set; }

        public DateTime? created { get; set; }

        public DateTime? changed { get; set; }

        public bool? durumu { get; set; }

        public int? Sira { get; set; }

        public Guid KategoriId { get; set; }
        public Guid? LanguageId { get; set; }
        public int? SayfaTipi { get; set; }

        public string Baslik { get; set; }

        public string Url { get; set; }

        public string Aciklama { get; set; }

        public string Icerik { get; set; }

        public string Resim { get; set; }

        public string VideoUrl { get; set; }

        public string SeoBaslik { get; set; }

        public string SeoAciklama { get; set; }

        public string SeoAnahtarKelime { get; set; }
        public string KategoriAdi { get; set; }
        public string KategoriUrl { get; set; }
        public bool? Vitrin { get; set; }
        public int? Hit { get; set; }
        public string Dosya { get; set; }
        public bool? Slider { get; set; }
        public DateTime? ZamanlanmisTarih { get; set; }
    }
}