using Ikriv.Xml;
using Newtonsoft.Json;
using ScreenSlicer.SettingsConverters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private static JsonSerializer _serializer;

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
                if (_settingsWindow != null)
                    _settingsWindow.PropertyChanged -= SaveByPropertyChanged;
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
                if (_localization != null)
                    _localization.PropertyChanged -= SaveByPropertyChanged;
                _localization = value;
                _localization.PropertyChanged += SaveByPropertyChanged;
            }
        }

        private SnapSettings _snaps;
        public SnapSettings Snaps
        {
            get
            {
                if (_snaps == null)
                    Snaps = new SnapSettings();
                return _snaps;
            }
            set
            {
                if (_snaps != null)
                    _snaps.PropertyChanged -= SaveByPropertyChanged;
                _snaps = value;
                _snaps.PropertyChanged += SaveByPropertyChanged;
            }
        }

        private RegionSettings _regions;
        public RegionSettings Regions
        {
            get
            {
                if (_regions == null)
                    Regions = new RegionSettings() { MinRegionSize = new System.Drawing.Size(200, 60) };
                return _regions;
            }
            set
            {
                if (_regions != null)
                    _regions.PropertyChanged -= SaveByPropertyChanged;
                _regions = value;
                _regions.PropertyChanged += SaveByPropertyChanged;
            }
        }

        public Settings() { }
        private Settings(bool defaultValues)
        {
            if (!defaultValues)
                return;
            Localization = new LocalizationSettings() { Culture = CultureInfo.GetCultureInfo("en") };
            Snaps = new SnapSettings() { SnapDistance = 10, SnapToMonitors = true, SnapToRegions = true };
        }

        private static Settings Load()
        {
            if (File.Exists(SettingsPath))
            {
                using (JsonTextReader file = new JsonTextReader(File.OpenText(SettingsPath)))
                {
                    var serializer = GetSerializer();
                    try
                    {
                        return serializer.Deserialize<Settings>(file);
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Message, "settings deserialization fault");
                    }
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

            using (StreamWriter file = File.CreateText(SettingsPath))
            {
                var serializer = GetSerializer();
                try
                {
                    serializer.Serialize(file, Instance);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "settings serialization fault");
                }
            }
        }

        private static JsonSerializer GetSerializer()
        {
            if (_serializer == null)
            {
                _serializer = new JsonSerializer()
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                };
                _serializer.Converters.Add(RectangleConverter.Default);
                _serializer.Converters.Add(PointConverter.Default);
                _serializer.Converters.Add(SizeConverter.Default);
            }
            return _serializer;
        }
    }
}
