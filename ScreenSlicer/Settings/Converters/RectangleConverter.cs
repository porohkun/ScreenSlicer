using Newtonsoft.Json;
using System;
using System.Drawing;

namespace ScreenSlicer.SettingsConverters
{
    public class RectangleConverter : JsonConverter<Rectangle>
    {
        public static RectangleConverter Default { get; } = new RectangleConverter();

        enum Props
        {
            X,
            Y,
            Width,
            Height,
            W = Width,
            H = Height
        }

        public override Rectangle ReadJson(JsonReader reader, Type objectType, Rectangle existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var data = (string)reader.Value;
            var parts = data.Split(';');
            int x = 0, y = 0, w = 0, h = 0;

            foreach (var part in parts)
            {
                var s = part.Split(':');
                if (s.Length == 2)
                    if (Enum.TryParse(s[0].Trim(), true, out Props prop))
                        if (Int32.TryParse(s[1].Trim(), out int value))
                            switch (prop)
                            {
                                case Props.X: x = value; continue;
                                case Props.Y: y = value; continue;
                                case Props.W: w = value; continue;
                                case Props.H: h = value; continue;
                            }
                throw new ArgumentException();
            }

            return new Rectangle(x, y, w, h);
        }

        public override void WriteJson(JsonWriter writer, Rectangle value, JsonSerializer serializer)
        {
            writer.WriteValue($"x:{value.X}; y:{value.Y}; width:{value.Width}; height:{value.Height}");
        }
    }
}
