using System.Collections;
using System.Windows;
using WPFLocalizeExtension.Extensions;

namespace ScreenSlicer.Commands
{
    public class RemoveItemCommand : InjectableCommand<RemoveItemCommand>
    {
        const string PopupMessage = "RemoveItemCommand.Popup.Message";
        const string PopupCaption = "RemoveItemCommand.Popup.Caption";

        protected override bool CanExecuteInternal(object parameter)
        {
            if (parameter == null)
                return false;
            if (!(parameter is object[] param))
                return false;
            if (param.Length != 2 && param.Length != 3)
                return false;
            if (param[0] == null || param[1] == null)
                return false;
            if (param[0] is IList)
                return true;
            return false;
        }

        protected override void ExecuteInternal(object parameter)
        {
            if (parameter is object[] param)
            {
                if (param.Length == 2 ||
                    (param.Length == 3 && MessageBox.Show(string.Format(LocValue(PopupMessage), param[2]), LocValue(PopupCaption), MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    var list = param[0] as IList;
                    var item = param[1];
                    list.Remove(item);
                }
            }
        }

        private static string LocValue(string key)
        {
            return LocExtension.GetLocalizedValue<string>(key, Settings.Instance.Localization.Culture);
        }
    }
}
