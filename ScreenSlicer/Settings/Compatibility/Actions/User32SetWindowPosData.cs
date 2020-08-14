using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScreenSlicer.Native;

namespace ScreenSlicer.Compatibility.Actions
{
    public class User32SetWindowPosData : ActionDataBase
    {
        private ShowWindowPosition _flags;

        [JsonConverter(typeof(StringEnumConverter))]
        public ShowWindowPosition Flags
        {
            get => _flags;
            set
            {
                if (_flags != value)
                {
                    _flags = value;
                    NotifyPropertyChanged(nameof(Flags));
                }
            }
        }

        public override object Clone()
        {
            return new User32SetWindowPosData()
            {
                Flags = Flags
            };
        }
    }
}
