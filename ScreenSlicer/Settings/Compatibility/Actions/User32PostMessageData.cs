using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScreenSlicer.Native;

namespace ScreenSlicer.Compatibility.Actions
{
    public class User32PostMessageData : ActionDataBase
    {
        private WindowMessage _message;

        [JsonConverter(typeof(StringEnumConverter))]
        public WindowMessage Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChanged(nameof(Message));
                }
            }
        }

        public override object Clone()
        {
            return new User32PostMessageData()
            {
                Message = Message
            };
        }
    }
}
