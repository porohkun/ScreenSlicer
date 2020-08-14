using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using WPFLocalizeExtension.Engine;

namespace ScreenSlicer.SettingsConverters
{
    public class CultureInfoConverter : JsonConverter<CultureInfo>
    {
        public static CultureInfoConverter Default { get; } = new CultureInfoConverter();

        CultureInfoConverter() { }

        public override CultureInfo ReadJson(JsonReader reader, Type objectType, CultureInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var data = (string)reader.Value;
            var culture = LocalizeDictionary.Instance.MergedAvailableCultures.FirstOrDefault(c => c.Name == data);
            if (culture == null)
                culture = CultureInfo.GetCultureInfo(data);
            if (culture == null)
                culture = CultureInfo.CurrentCulture;
            return culture;
        }

        public override void WriteJson(JsonWriter writer, CultureInfo value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Name);
        }
    }
}
