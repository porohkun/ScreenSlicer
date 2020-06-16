using Newtonsoft.Json;
using System;
using System.Drawing;

namespace ScreenSlicer.SettingsConverters
{
    public class PointConverter : JsonConverter<Point>
    {
        public static PointConverter Default { get; } = new PointConverter();

        enum Props
        {
            X,
            Y
        }

        public override Point ReadJson(JsonReader reader, Type objectType, Point existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var data = (string)reader.Value;
            var parts = data.Split(';');
            int x = 0, y = 0;

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
                            }
                throw new ArgumentException();
            }

            return new Point(x, y);
        }

        public override void WriteJson(JsonWriter writer, Point value, JsonSerializer serializer)
        {
            writer.WriteValue($"x:{value.X}; y:{value.Y}");
        }
    }
}
