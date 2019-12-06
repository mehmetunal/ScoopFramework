using System;
using System.Globalization;

namespace ScoopFramework.TTGenerators.Helper
{
    public class TarihConverter : Newtonsoft.Json.Converters.DateTimeConverterBase
    {

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;
            return DateTime.ParseExact(reader.Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("dd.MM.yyyy"));
        }

    }
}
