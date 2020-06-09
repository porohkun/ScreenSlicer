using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;
using Ikriv.Xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
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
            var serializer = GetSerializer();
            if (File.Exists(SettingsPath))
            {
                using (var stream = File.OpenRead(SettingsPath))
                {
                    try
                    {
                        return serializer.Deserialize<Settings>(stream);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
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
            var serializer = GetSerializer();
            try
            {
                using (var stream = File.Open(SettingsPath, FileMode.Create))
                {
                    serializer.Serialize(new XmlWriterSettings() { Indent = true }, stream, Instance);
                }
            }
            catch { }
        }

        static IExtendedXmlSerializer GetSerializer()
        {
            return new ConfigurationContainer()
                .Type<System.Drawing.Rectangle>()
                    .Member(x => x.X).Name("X").Attribute()
                    .Member(x => x.Y).Name("Y").Attribute()
                    .Member(x => x.Width).Name("Width").Attribute()
                    .Member(x => x.Height).Name("Height").Attribute()
                .Type<System.Drawing.Point>()
                    .Member(x => x.X).Name("X").Attribute()
                    .Member(x => x.Y).Name("Y").Attribute()
                .Type<System.Drawing.Size>()
                    .Member(x => x.Width).Name("Width").Attribute()
                    .Member(x => x.Height).Name("Height").Attribute()
                    .Create();
        }

        static XmlAttributeOverrides GetOverrides()
        {
            return new OverrideXml()
                .Override<System.Drawing.Rectangle>()
                    .Member(nameof(System.Drawing.Rectangle.X)).XmlAttribute()
                    .Member(nameof(System.Drawing.Rectangle.Y)).XmlAttribute()
                    .Member(nameof(System.Drawing.Rectangle.Width)).XmlAttribute()
                    .Member(nameof(System.Drawing.Rectangle.Height)).XmlAttribute()
                    .Member(nameof(System.Drawing.Rectangle.Location)).XmlIgnore()
                    .Member(nameof(System.Drawing.Rectangle.Size)).XmlIgnore()
                .Override<System.Drawing.Point>()
                    .Member(nameof(System.Drawing.Point.X)).XmlAttribute()
                    .Member(nameof(System.Drawing.Point.Y)).XmlAttribute()
                .Override<System.Drawing.Size>()
                    .Member(nameof(System.Drawing.Size.Width)).XmlAttribute()
                    .Member(nameof(System.Drawing.Size.Height)).XmlAttribute()
                .Commit();
        }
    }
}
