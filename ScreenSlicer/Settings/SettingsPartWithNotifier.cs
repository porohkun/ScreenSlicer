using System.ComponentModel;

namespace ScreenSlicer
{
    public interface ISettingsPartWithNotifier
    {
        bool NotifierSubscribed { get; }
        void SubscribeNotifier(PropertyChangedEventHandler handler);
        void UnsubscribeNotifier(PropertyChangedEventHandler handler);
    }

    public abstract class SettingsPartWithNotifier : INotifyPropertyChanged, ISettingsPartWithNotifier
    {
        bool _notifierSubscribed;
        bool ISettingsPartWithNotifier.NotifierSubscribed => _notifierSubscribed;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void ISettingsPartWithNotifier.SubscribeNotifier(PropertyChangedEventHandler handler)
        {
            PropertyChanged += handler;
            _notifierSubscribed = true;
        }

        void ISettingsPartWithNotifier.UnsubscribeNotifier(PropertyChangedEventHandler handler)
        {
            PropertyChanged -= handler;
            _notifierSubscribed = false;
        }
    }
}