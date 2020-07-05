using Newtonsoft.Json;

namespace ScreenSlicer.Compatibility.Actions
{
    public class User32MoveWindowData : ActionDataBase
    {
        private bool _shouldRepaint;

        [JsonProperty(nameof(ShouldRepaint))]
        public bool ShouldRepaint
        {
            get => _shouldRepaint;
            set
            {
                if (_shouldRepaint != value)
                {
                    _shouldRepaint = value;
                    NotifyPropertyChanged(nameof(ShouldRepaint));
                }
            }
        }
    }
}
