using Newtonsoft.Json;
using System;
using System.Drawing;

namespace ScreenSlicer.SettingsConverters
{
    public class SizeConverter : JsonConverter<Size>
    {
        public static SizeConverter Default { get; } = new SizeConverter();

        enum Props
        {
            Width,
            Height,
            W = Width,
            H = Height
        }

        public override Size ReadJson(JsonReader reader, Type objectType, Size existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var data = (string)reader.Value;
            var parts = data.Split(';');
            int w = 0, h = 0;

            foreach (var part in parts)
            {
                var s = part.Split(':');
                if (s.Length == 2)
                    if (Enum.TryParse(s[0].Trim(), true, out Props prop))
                        if (Int32.TryParse(s[1].Trim(), out int value))
                            switch (prop)
                            {
                                case Props.W: w = value; continue;
                                case Props.H: h = value; continue;
                            }
                throw new ArgumentException();
            }

            return new Size(w, h);
        }

        public override void WriteJson(JsonWriter writer, Size value, JsonSerializer serializer)
        {
            writer.WriteValue($"width:{value.Width}; height:{value.Height}");
        }
    }
}
