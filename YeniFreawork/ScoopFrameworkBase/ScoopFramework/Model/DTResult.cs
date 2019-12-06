using System.Collections.Generic;

namespace ScoopFramework.Model
{
    public class DTResult<T>
    {
        public int draw { get; set; }

        /// Veritabanındaki toplam kayıtsayısı 
        public int recordsTotal { get; set; }

        /// Filtrelemeden sonraki toplam kayıt sayısı  
        public int recordsFiltered { get; set; }

        // Tabloda görüntülenecek veri.
        // BU paramaetre ismi ajaxDT ayarlarının  dataSrc özelliği ile değiştirlebilir
        public List<T> data { get; set; }
    }

    // Otomatik işlem yapmak için gönderien ek kolon 
    public abstract class DTRow
    {
        //dt-tag tr  düğümünün id değeri buraya verilir
        public virtual string DT_RowId
        {
            get { return null; }
        }

        public virtual string DT_RowClass { get { return null; } }

        public virtual object DT_RowData { get { return null; } }
    }

    //Ajax ile server tarafınagidecek veriler bu sınıftatanımlanmıştır.
    public class DTParameters
    {
        public int Draw { get; set; }

        // Tablonun kolon adlarını içeren dizi
        public DTColumn[] Columns { get; set; }

        /// Kaç tane kolon  sıralanacak, bir eleman varsa tek bir  alana göre sıralanacak.Birden fazlaysa multi-column sort uygulanacak
        public DTOrder[] Order { get; set; }

        // Skip() ' te  kullanılacak veri (current dataset in başlangıç noktası )
        public int Start { get; set; }

        //o filtreden sonra gösterilecek satır sayısı 
        public int Length { get; set; }

        //Bütün alanlara uygulanan search verisi
        public DTSearch Search { get; set; }

        // İlk sıralama kolonunun  daha ileri sıralanması için kulanılır
        public string SortOrder
        {
            get
            {
                return Columns != null && Order != null && Order.Length > 0
                    ? (Columns[Order[0].Column].Data + (Order[0].Dir == DTOrderDir.DESC ? " " + Order[0].Dir : string.Empty))
                    : null;
            }
        }
    }

    //Ajax sorgusu yaparken gönderilen search bilgisi
    // A search, as sent by jQuery DataTables when doing AJAX queries.
    public class DTSearch
    {
        // Bütün alanlara uygulanan  search verisi
        public string Value { get; set; }

        //Arama için kullanılacak Regex verisi. Çok büük verisetlerini filtrlemekte kullanılabilir.
        public bool Regex { get; set; }
    }
    //  jQuery DataTables column.
    public class DTColumn
    {
        //columns.data  ile tanımlanan kolon verisi 
        public string Data { get; set; }

        // columns.name ile tanımlanan column adi
        public string Name { get; set; }

        //Column searchable mı değil mi anlamında 
        public bool Searchable { get; set; }

        /// Kolon sıralama enabled mı ?  columns.orderable ile ayarlanır
        public bool Orderable { get; set; }

    }
    //Sıralama bilgisi
    public class DTOrder
    {
        // Sıralamada baz alınacak kolon
        public int Column { get; set; }

        // OSıralama Yönü , asc  -   desc
        public DTOrderDir Dir { get; set; }
    }
    public enum DTOrderDir
    {
        ASC,
        DESC
    }
}
