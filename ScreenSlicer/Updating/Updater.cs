using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenSlicer.Updating
{
    public class Updater
    {
        private readonly AutoResetEvent _autoEvent;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        private readonly Timer _timer;
        private DateTime _lastUpdateCheck = DateTime.Now;
        private TimeSpan _updateFrequency = new TimeSpan(1, 0, 0);

        public Updater()
        {
#if DEBUG
            return;
#endif
            _autoEvent = new AutoResetEvent(false);
            _timer = new Timer(CheckStatus, _autoEvent, 60000, 5000);
        }

        private void CheckStatus(object stateInfo)
        {
            if (_lastUpdateCheck + _updateFrequency < DateTime.Now)
                CheckUpdates();
        }

        public async void CheckUpdates()
        {
            _lastUpdateCheck = DateTime.Now;

            try
            {
                using (var mgr = new UpdateManager(@"https://github.com/porohkun/ScreenSlicer/releases/latest/download/"))
                {
                    SquirrelAwareApp.HandleEvents(
                        onInitialInstall: v =>
                        {
                            mgr.CreateShortcutForThisExe();
                            mgr.CreateRunAtWindowsStartupRegistry();
                        },
                        onAppUpdate: v => mgr.CreateShortcutForThisExe(),
                        onAppUninstall: v =>
                        {
                            mgr.RemoveShortcutForThisExe();
                            mgr.RemoveRunAtWindowsStartupRegistry();
                        },
                        onFirstRun: () => { });

                    if (!true/*Updates.Enabled*/) //updates are not enabled, skipping update
                        return;
                    if (!mgr.IsInstalledApp) //not installed during Squirrel, skipping update
                        return;

                    try
                    {
                        var installedVersion = mgr.CurrentlyInstalledVersion();
                        var entry = await mgr.UpdateApp();
                        if (entry != null && (installedVersion == null || entry.Version > installedVersion))
                        {
                            MessageBox.Show("You need to restart app for apply changes", "Screen Slicer have been updated", MessageBoxButton.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorMessage = $"{ex.Message}\r\n\r\n{ex.StackTrace}\r\n\r\nCopy error message to clipboard?";
                        if (MessageBox.Show(errorMessage, "Error in update/install process", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            Clipboard.SetText(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message}\r\n\r\n{ex.StackTrace}\r\n\r\nCopy error message to clipboard?";
                if (MessageBox.Show(errorMessage, "Error before update/install process", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    Clipboard.SetText(errorMessage);
            }
        }
    }
}
