using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer
{
    class MainModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<NotifyIcon.NotifyIcon>().ToSelf().InSingletonScope();
            Bind<Windows.SettingsWindow>().ToSelf().InSingletonScope();
            Bind<Windows.SlicingWindow>().ToSelf().InSingletonScope();
            Bind<Windows.WinListWindow>().ToSelf().InSingletonScope();
            Bind<Updating.Updater>().ToSelf().InSingletonScope();
            Bind<Managers.RegionsManager>().ToSelf().InSingletonScope();
            Bind<Managers.ProcessesWatcher>().ToSelf().InSingletonScope();
        }
    }
}
