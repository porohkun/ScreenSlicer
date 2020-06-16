using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

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
        public static string SettingsPath => Path.Combine(AppDataPath, "settings.json");

        public static Settings Instance { get; set; } = Load();

        private static JsonSerializer _serializer;

        private event Action PropertyChanged;

        [XmlElement(nameof(SettingsWindow))]
        private WindowStateSettings _settingsWindow;

        [XmlElement(nameof(Localization))]
        private LocalizationSettings _localization;

        [XmlElement(nameof(Snaps))]
        private SnapSettings _snaps;

        [XmlElement(nameof(Regions))]
        private RegionSettings _regions;

        [XmlElement(nameof(Compatibility))]
        private CompatibilitySettings _compatibility;

        public WindowStateSettings SettingsWindow
        {
            get
            {
                _settingsWindow = CheckSettingsPartExistAndSubscribe<WindowStateSettings>(_settingsWindow);
                return _settingsWindow;
            }
        }

        public LocalizationSettings Localization
        {
            get
            {
                _localization = CheckSettingsPartExistAndSubscribe<LocalizationSettings>(_localization);
                return _localization;
            }
        }

        public SnapSettings Snaps
        {
            get
            {
                _snaps = CheckSettingsPartExistAndSubscribe<SnapSettings>(_snaps);
                return _snaps;
            }
        }

        public RegionSettings Regions
        {
            get
            {
                _regions = CheckSettingsPartExistAndSubscribe<RegionSettings>(_regions);
                return _regions;
            }
        }

        public CompatibilitySettings Compatibility
        {
            get
            {
                _compatibility = CheckSettingsPartExistAndSubscribe<CompatibilitySettings>(_compatibility);
                return _compatibility;
            }
        }

        public Settings() { }
        private Settings(bool defaultValues)
        {
            if (!defaultValues)
                return;
            //Localization = new LocalizationSettings() { Culture = CultureInfo.GetCultureInfo("en") };
        }

        private T CheckSettingsPartExistAndSubscribe<T>(ISettingsPartWithNotifier part) where T : SettingsPartWithNotifier
        {
            if (part == null)
                part = Activator.CreateInstance<T>();
            else if (!(part is T))
                throw new ArgumentException();
            if (!part.NotifierSubscribed)
                part.SubscribeNotifier(PartChanged);
            return (T)part;
        }

        private void PartChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke();
        }

        private static Settings Load()
        {
            Settings result = null;
            if (File.Exists(SettingsPath))
            {
                using (JsonTextReader file = new JsonTextReader(File.OpenText(SettingsPath)))
                {
                    var serializer = GetSerializer();
                    try
                    {
                        result = serializer.Deserialize<Settings>(file);
                        if (result == null)
                            throw new SerializationException("cant read settings.xml");
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Message, "settings deserialization fault");
                    }
                }
            }
            result = result ?? new Settings(true);
            result.PropertyChanged += Save;
            return result;
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
                _serializer.Converters.Add(SettingsConverters.RectangleConverter.Default);
                _serializer.Converters.Add(SettingsConverters.PointConverter.Default);
                _serializer.Converters.Add(SettingsConverters.SizeConverter.Default);
                _serializer.Converters.Add(SettingsConverters.CultureInfoConverter.Default);
            }
            return _serializer;
        }
    }
}
