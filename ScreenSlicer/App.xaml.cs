﻿using Hardcodet.Wpf.TaskbarNotification;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenSlicer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        private TaskbarIcon _notifyIcon;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConfigureContainer();
            ComposeObjects();
            _notifyIcon.BeginInit();
        }

        private void ConfigureContainer()
        {
            _container = new StandardKernel();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = _container.Get<MainWindow>();
            _notifyIcon = _container.Get<NotifyIcon.NotifyIcon>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
