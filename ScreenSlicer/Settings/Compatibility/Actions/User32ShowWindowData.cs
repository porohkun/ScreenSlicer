using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScreenSlicer.Native;

namespace ScreenSlicer.Compatibility.Actions
{
    public class User32ShowWindowData : ActionDataBase
    {
        private ShowWindowCommand _command;

        [JsonConverter(typeof(StringEnumConverter))]
        public ShowWindowCommand Command
        {
            get => _command;
            set
            {
                if (_command != value)
                {
                    _command = value;
                    NotifyPropertyChanged(nameof(Command));
                }
            }
        }
    }
}
