﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using WPFLocalizeExtension.Engine;

namespace ScreenSlicer
{

    [Serializable]
    public class Settings
    {
        public static Version Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public static string AppDataPath =>
#if DEBUG
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
#else
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScreenSlicer");
#endif
        public static string SettingsPath => Path.Combine(AppDataPath, "settings.xml");

        public static Settings Instance { get; set; } = Load();

        private WindowStateSettings _settingsWindow;
        public WindowStateSettings SettingsWindow
        {
            get
            {
                if (_settingsWindow == null)
                    SettingsWindow = new WindowStateSettings() { Width = 640, Height = 480 };
                return _settingsWindow;
            }
            set
            {
                _settingsWindow = value;
                _settingsWindow.PropertyChanged += SaveByPropertyChanged;
            }
        }

        private LocalizationSettings _localization;
        public LocalizationSettings Localization
        {
            get
            {
                if (_localization == null)
                    Localization = new LocalizationSettings();
                return _localization;
            }
            set
            {
                _localization = value;
                _localization.PropertyChanged += SaveByPropertyChanged;
            }
        }


        public Settings() { }
        private Settings(bool defaultValues)
        {
            if (defaultValues == false)
                return;
            Localization = new LocalizationSettings() { Culture = CultureInfo.GetCultureInfo("de") };
        }

        private static Settings Load()
        {
            var serializer = new XmlSerializer(typeof(Settings));
            if (File.Exists(SettingsPath))
            {
                using (var stream = File.OpenRead(SettingsPath))
                {
                    try
                    {
                        return (Settings)serializer.Deserialize(stream);
                    }
                    catch { }
                }
            }
            return new Settings(true);
        }

        private void SaveByPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Save();
        }

        public static void Save()
        {
            if (!Directory.Exists(AppDataPath))
                Directory.CreateDirectory(AppDataPath);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            try
            {
                using (var stream = File.Open(SettingsPath, FileMode.Create))
                {
                    serializer.Serialize(stream, Instance);
                }
            }
            catch { }
        }
    }
}