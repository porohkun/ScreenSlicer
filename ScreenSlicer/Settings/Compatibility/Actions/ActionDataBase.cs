using System.ComponentModel;

namespace ScreenSlicer.Compatibility.Actions
{
    public abstract class ActionDataBase : IActionData
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
